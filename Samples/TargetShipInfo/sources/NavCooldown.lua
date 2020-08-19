local m_myMainForm = nil
local m_navigatorForm = nil		
local m_navigatorBtn = nil		

local m_template = nil
local m_reactions={}

local m_navigatorPanel = nil
local m_currTransportID = nil

local m_templateActionsItem = {}
local m_emptyHeaderTxt = "Нет цели"
local m_headerTxt = m_emptyHeaderTxt
local VERY_BIG_CD = 1000000
local m_targetLost = false

local m_oldNavigatorPanel = nil
local m_oldTransportID = nil

local function GetTimestamp()
	return common.GetMsFromDateTime( common.GetLocalDateTime() )
end

local function ClearWidgets()
	for i=0, 12 do
		hide(m_templateActionsItem[i].widget)
	end
	m_headerTxt = m_emptyHeaderTxt
	local header = m_navigatorForm:GetChildChecked("txtHeader", false)
	setText(header, m_headerTxt, "ColorWhite",  "center", 12)
	m_navigatorPanel = nil
	m_currTransportID = nil
end

local function CreateTemplates()
	for i=0, 12 do
		setTemplateWidget(m_navigatorForm)
		local result = {}
		result.widget = createWidget(m_navigatorForm, "sp"..i, "SpellView", WIDGET_ALIGN_LOW, WIDGET_ALIGN_LOW, 50, 70, 10+i*60, 22)
		result.Image = result.widget:GetChildChecked("Image", false)
		result.Cooldown = result.widget:GetChildChecked("Cooldown", false)
		result.ProgressBar = result.widget:GetChildChecked("ProgressBar", false)
		
		m_templateActionsItem[i] = result
		hide(result.widget)
	end
end

local function UpdateProgreesBar(aMin, aMax, aCurr, aProgressBar)
	if aMax == aMin then
		return
	end
	local minimum, maximum, value = aMin, aMax, aCurr
	local realRect = aProgressBar:GetRealRect()
	local backgroundWidget = aProgressBar:GetChildChecked("Background", false)
	local origPlacement = backgroundWidget:GetPlacementPlain ()
	local placement = CopyTable(origPlacement)
	placement.alignX = WIDGET_ALIGN_LOW_ABS
	local percent = (value - minimum) / (maximum - minimum)
	placement.sizeX = (realRect.x2 - realRect.x1) * percent
	backgroundWidget:PlayResizeEffect(origPlacement, placement, 150, EA_MONOTONOUS_INCREASE)
	backgroundWidget:SetBackgroundColor( { r = 0.105, g = 0.367, b = 0.078, a = 1.0 })
end

local function HideActionCooldown(aCurrAction)
	hide(aCurrAction.Cooldown)
	hide(aCurrAction.ProgressBar)
	setFade(aCurrAction.Image, 1.0)
end

local function ShowActionCooldown(aCurrAction, aDuration, aRemaining)
	aCurrAction.Cooldown:SetVal("text", userMods.ToWString(FormatTimer(aRemaining, "mm:sc")))
	UpdateProgreesBar(0, aDuration, aRemaining, aCurrAction.ProgressBar)
	show(aCurrAction.Cooldown)
	show(aCurrAction.ProgressBar)
	setFade(aCurrAction.Image, 0.5)
end

local function UpdateCooldown(aNeedDelta)
	local result = false
	for i = 0, GetTableSize( m_navigatorPanel ) - 1 do
		local currActionGUI = m_templateActionsItem[i]
		local currActionInfo = m_navigatorPanel[i]

		local remaining = currActionInfo.lastRemaining
		if aNeedDelta and currActionInfo.lastRemaining ~= VERY_BIG_CD then 
			local delta = 0
			delta = GetTimestamp() - currActionInfo.lastTimestamp 
			remaining = currActionInfo.lastRemaining - delta
			if remaining < 0 then 
				remaining = 0
			end
			currActionInfo.lastRemaining = remaining
			currActionInfo.lastTimestamp = GetTimestamp()
		end
		
		if remaining == 0 or remaining == VERY_BIG_CD then
			m_navigatorPanel[i].lastRemaining = VERY_BIG_CD
			HideActionCooldown(currActionGUI)
		else
			ShowActionCooldown(currActionGUI, currActionInfo.lastDuration, remaining)
			result = true
		end	
	end
	return result
end

local function CheckCheatForAction(aCurrAction, aRemaining)
	if aCurrAction.lastRemaining < aRemaining and aCurrAction.lastRemaining > 2000 then 
		local header = m_navigatorForm:GetChildChecked("txtHeader", false)
		setText(header, m_headerTxt.." !!!Возможен чит, см mod.txt", "ColorWhite",  "center", 12)
		LogInfo("Find increase cd time, last currAction.lastRemaining =  ", aCurrAction.lastRemaining, "ms current = ", aRemaining)
	end
end

local function CheckCheat(anIndex, aRemaining)
	CheckCheatForAction(m_navigatorPanel[anIndex], aRemaining)

	if m_oldTransportID == m_currTransportID and m_oldNavigatorPanel then
		CheckCheatForAction(m_oldNavigatorPanel[anIndex], aRemaining)
	end
end

local function UpdatePanel()
	if not isVisible(m_navigatorForm) or not isVisible(mainForm) then
		return
	end
	local transportId = avatar.GetObservedTransport()
	
	if not transportId or not transport.CanDrawInterface(transportId)then 
		m_targetLost = true
		local header = m_navigatorForm:GetChildChecked("txtHeader", false)
		if m_headerTxt ~= m_emptyHeaderTxt then
			setText(header, "(ПОТЕРЯН) "..m_headerTxt, "ColorWhite",  "center", 12)
		end
		m_oldTransportID = m_currTransportID
		m_oldNavigatorPanel = m_navigatorPanel
		if not UpdateCooldown(true) then 
			ClearWidgets()
		end
	else
		local devices = transport.GetDevices( transportId )

		for k, deviceId in pairs( devices ) do
			local deviceType = device.GetUsableDeviceType( deviceId )
			if deviceType == USDEV_NAVIGATOR then			
				local info = avatar.GetUsableDeviceInfo( deviceId )

				for i = 0, GetTableSize( info.actions ) - 1 do
					local action = info.actions[i]
					--create
					if not m_navigatorPanel or m_targetLost or (m_currTransportID ~= transportId and m_currTransportID) then 
						ClearWidgets()
						m_targetLost = false
						
						m_navigatorPanel = {}
						local shipInfo = transport.GetShipInfo( transportId )

						m_headerTxt = userMods.FromWString(shipInfo.name)
						local header = m_navigatorForm:GetChildChecked("txtHeader", false)
						setText(header, m_headerTxt, "ColorWhite",  "center", 12)	
					end
					if not m_navigatorPanel[i] then
						setTemplateWidget(m_navigatorForm)
						local result = m_templateActionsItem[i]
						result.Image:SetBackgroundTexture(action.image)				
						show(result.widget)
						m_navigatorPanel[i] = {}
						m_navigatorPanel[i].lastRemaining = VERY_BIG_CD
					end
					--update
					local cooldown = device.GetCooldown( deviceId, i )

					if cooldown then		  
						local remaining = cooldown.remainingMs	

						CheckCheat(i, remaining)
						m_navigatorPanel[i].lastRemaining = remaining
						m_navigatorPanel[i].lastDuration = cooldown.durationMs
						m_navigatorPanel[i].lastTimestamp = GetTimestamp()
					end
				end
				m_currTransportID = transportId
				UpdateCooldown(false)
				m_oldTransportID = nil
				m_oldNavigatorPanel = nil
			end
		end	
		
	end
end

function AvatarShipChanged()
	local myShipID = unit.GetTransport(avatar.GetId())
	if myShipID then
		show(mainForm)
	else
		hide(mainForm)
	end
end

function ChangeNavWndVisible()
	if not isVisible(m_navigatorForm) then
		show(m_navigatorForm)
	else
		hide(m_navigatorForm)
	end
end

function AddReaction(name, func)
	if not m_reactions then m_reactions={} end
	m_reactions[name]=func
end

function RunReaction(widget)
	if DnD:IsDragging() then return end
	local name=getName(widget)
	if not name or not m_reactions or not m_reactions[name] then return end
	m_reactions[name]()
end

function ButtonPressed(params)
	RunReaction(params.widget)
	changeCheckBox(params.widget)
end

local function InitNavigatorForm()
	setTemplateWidget(m_template)
	
	local form=createWidget(m_myMainForm, "NavigatorForm", "Panel", WIDGET_ALIGN_LOW, WIDGET_ALIGN_LOW, 550, 94, 30, 0)
	priority(form, 5500)
	hide(form)

	setText(createWidget(form, "txtHeader", "TextView", nil, nil, 550, 25, 0, 4), m_emptyHeaderTxt, "ColorWhite",  "center", 12)
	return form
end

function InitNavCoolDown()
	if not g_showCoolDown then
		return
	end 
	m_template = createWidget(nil, "Template", "Template")
	setTemplateWidget(m_template)
	m_myMainForm = createWidget(mainForm, "NavigatorForm", "Panel", WIDGET_ALIGN_LOW, WIDGET_ALIGN_LOW, 580, 94, 660, 0)
	m_myMainForm:SetBackgroundColor({r=0;g=0;b=0;a=0})
	
	m_navigatorForm = InitNavigatorForm()
	
	m_navigatorBtn = createWidget(m_myMainForm, "NavigatorCDButton", "Button", WIDGET_ALIGN_LOW, WIDGET_ALIGN_LOW, 30, 25, 0, 0)
	setText(m_navigatorBtn, "NC")
	DnD:Init(m_myMainForm, m_navigatorBtn, true)
	
	common.RegisterReactionHandler(ButtonPressed, "execute")
	
	
	CreateTemplates()
	
	AddReaction("NavigatorCDButton", function () ChangeNavWndVisible() end)
	ChangeNavWndVisible()
	
	startTimer("updateTimer", "EVENT_UPADATE_PANEL_TIMER", 0.25)
	common.RegisterEventHandler(UpdatePanel, "EVENT_UPADATE_PANEL_TIMER")
	common.RegisterEventHandler(AvatarShipChanged, "EVENT_AVATAR_TRANSPORT_CHANGED")

	AvatarShipChanged()
end

if (avatar.IsExist()) then
	InitNavCoolDown()
else
	common.RegisterEventHandler(InitNavCoolDown, "EVENT_AVATAR_CREATED")
end
