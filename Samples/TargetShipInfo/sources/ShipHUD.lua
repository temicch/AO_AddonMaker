Global( "ShipId", nil)
Global( "Device", {})
Global( "Shield", {})
Global( "ShipPanel", mainForm:GetChildChecked("MainPanel", false))
Global( "ShipContainer", ShipPanel:GetChildChecked("ShipContainer", false))
Global( "Ship", ShipContainer:GetChildChecked("ShipPanel", false))
Global( "LCannon", Ship:GetChildChecked("LCannon", false):GetWidgetDesc())
Global( "RCannon", Ship:GetChildChecked("RCannon", false):GetWidgetDesc())
Global( "Artillery", Ship:GetChildChecked("Artillery", false):GetWidgetDesc())
Global( "Helm", Ship:GetChildChecked("Helm", false):GetWidgetDesc())
Global( "Turbine", Ship:GetChildChecked("Turbine", false):GetWidgetDesc())
Global( "Motor", Ship:GetChildChecked("Motor", false):GetWidgetDesc())
Global( "Reactor", Ship:GetChildChecked("Reactor", false):GetWidgetDesc())
Global( "Insight", Ship:GetChildChecked("Insight", false))
Global( "TCount", Ship:GetChildChecked("TCount", false))
Global( "TooltipText", Ship:GetChildChecked("Tooltip", false))
Global( "SHUD", userMods.GetGlobalConfigSection("TargetSHUD"))
local TIMER = 0
local number2 = 0
local m_shipNameHeader = ""
local m_shipNotFoundHeader = ""

Global("Loc", nil)
Global( "L", {
	[ "eng" ]  = {"chest","shipment"},
	[ "rus" ]  =  {"сундук","Сундук"},
})
-------------------------------------------------------------------------------
-- FUNCTION
-------------------------------------------------------------------------------
function ColorCheck( sender, health )
	sender:Show(true)
	health = math.abs(health)
	if health > 100 then
		health = 100
	end
	if SHUD["Gradual"] then
		local color = {r=(1 - (health/100));g=1;b=((health/100)^2);a=.9}
		if health >= 60 then
			color.g = 1.6 - (health/100)
		elseif health >= 40 then
			color.r = .8
			color.g = 1.4 - (health/100)
			color.b = 0
		elseif health ~= 0 then
			color.g = .3
		else
			color = {r=.3;b=.3;g=.3;a=.9}
		end
		sender:SetBackgroundColor(color)
	else
		local color
		if health == 100 then
			color = "100%"
		elseif health >= 75 then
			color = "80%"
		elseif health >= 50 then
			color = "60%"
		elseif health >= 25 then
			color = "40%"
		elseif health > 0 then
			color = "20%"
		else
			color = "0%"
		end
		if sender:GetBackgroundColor() ~= SHUD[color] then
			sender:SetBackgroundColor(SHUD[color])
		end
	end
end

function GetShipData(trans)
	if trans then
	Ship:Show(true)
		hide(m_shipNotFoundHeader)
		local shipInfo = transport.GetShipInfo( trans )
		setText(m_shipNameHeader, userMods.FromWString(shipInfo.name), "ColorWhite", "center")
		
		ShipId = trans
		local tHealthPer
		if transport.GetHealth then
			tHealthPer = math.ceil(transport.GetHealth(trans).value / transport.GetHealth(trans).limit * 100)
		else
			tHealthPer = object.GetHealthInfo(trans).valuePercents
		end
		Ship:GetChildChecked("Ship", false):SetPriority(0)
		ColorCheck( Ship:GetChildChecked("Ship", false), tHealthPer )
		local devs = transport.GetDevices(trans)
		local Can = 0
		local Art = 0
		local ECa = 0
		local Hel = 0
		local Tur = 0
		if Device ~= nil then
			for i, v in pairs(Device) do
				if v and not string.find(v:GetName(), "Shield") then
					v:DestroyWidget()
					Device[i] = nil
				end
			end
		end
		--Device = {}
		for i, v in pairs(devs) do
			DeviceCreate(v)
		end

		Insight:Show(false)
		TCount:Show(false)
		
	else
	Ship:Show(false)
	end
end

function DeviceCreate(v)
	local trans = avatar.GetObservedTransport()
	local idcheck = nil
	local devtype = device.GetUsableDeviceType(v)
	if devtype == USDEV_SHIELD then
		local slot = device.GetShipSlotInfo(v).interfaceSlot
		local side = device.GetShipSlotInfo(v).side
		Device[v] = Ship:GetChildChecked("Shield"..side..slot, false)
		Device[v]:Show(true)
		idcheck = true
	elseif devtype == USDEV_BEAM_CANNON then
		Device[v] = mainForm:CreateWidgetByDesc(Artillery)
		Ship:AddChild(Device[v])
		local P = Device[v]:GetPlacementPlain()
		P.posY = 60
		if device.GetShipSlotInfo(v).interfaceSlot == 1 then
			P.posX = 42
		else
			P.posX = 86
		end
		Device[v]:SetPlacementPlain(P)
		idcheck = true
	elseif devtype == USDEV_CANNON then
		local P
		local slot = device.GetShipSlotInfo(v).interfaceSlot
		local side = device.GetShipSlotInfo(v).side
		if slot <= 4 and side == 4 then
			Device[v] = mainForm:CreateWidgetByDesc(LCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posX = 29
			P.posY = 103 + ((slot - 1) * 25)
		elseif slot <= 4 and side == 5 then
			Device[v] = mainForm:CreateWidgetByDesc(RCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posY = 103 + ((slot - 1) * 25)
			P.posX = 100
		elseif slot > 8 and side == 4 then
			Device[v] = mainForm:CreateWidgetByDesc(LCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posX = 29
			if slot == 9 then
				P.posY = 208
			else
				P.posX = 42
				P.posY = 256
			end
		elseif slot > 8 and side == 5 then
			Device[v] = mainForm:CreateWidgetByDesc(RCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posX = 100
			if slot == 9 then
				P.posY = 208
			else
				P.posX = 87
				P.posY = 256
			end
		elseif slot > 4 and side == 4 then
			Device[v] = mainForm:CreateWidgetByDesc(LCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posY = 118 + ((8 - slot) * 25)
			P.posX = 40
		elseif slot > 4 and side == 5 then
			Device[v] = mainForm:CreateWidgetByDesc(RCannon)
			Ship:AddChild(Device[v])
			P = Device[v]:GetPlacementPlain()
			P.posY = 118 + ((slot - 5) * 25)
			P.posX = 87
		end
		Device[v]:SetPlacementPlain(P)
		idcheck = true
	elseif devtype == USDEV_RUDDER then
		Device[v] = mainForm:CreateWidgetByDesc(Helm)
		Ship:AddChild(Device[v])
		idcheck = true
	elseif devtype == USDEV_ENGINE_VERTICAL then
		Device[v] = mainForm:CreateWidgetByDesc(Turbine)
		Ship:AddChild(Device[v])
		idcheck = true
	elseif devtype == USDEV_ENGINE_HORIZONTAL then
		Device[v] = mainForm:CreateWidgetByDesc(Motor)
		Ship:AddChild(Device[v])
		idcheck = true
	elseif devtype == USDEV_REACTOR then
		Device[v] = mainForm:CreateWidgetByDesc(Reactor)
		Ship:AddChild(Device[v])
		ColorCheck(Device[v], math.ceil((transport.GetEnergy(trans).limit - transport.GetEnergy(trans).value) / transport.GetEnergy(trans).limit * 100))
		Ship:Show(true)
	end
	if idcheck then
		local dHealthPer
		if device.GetHealth then
			dHealthPer = math.ceil(device.GetHealth(v).value / device.GetHealth(v).limit * 100)
		else
			if object.GetHealthInfo(v) then
				if object.GetHealthInfo(v).valuePercents then
					dHealthPer = object.GetHealthInfo(v).valuePercents
				elseif object.GetHealthInfo(v).value and object.GetHealthInfo(v).limit then
					dHealthPer = math.ceil(object.GetHealth(v).value / object.GetHealth(v).limit * 100)
				end
			elseif device.GetShieldStrength(v) then
				dHealthPer = math.ceil(device.GetShieldStrength(v).value / device.GetShieldStrength(v).maxValue * 100)
			end
		end
		if dHealthPer then
			ColorCheck(Device[v], dHealthPer)
		end
	end
end

function DeviceHealthChanged( params )
	if Device[params.id] and object.IsExist(params.id) then
		local dHealthPer
		if transport.GetHealth then
			dHealthPer = math.ceil(device.GetHealth(params.id).value / device.GetHealth(params.id).limit * 100)
		else
			if object.GetHealthInfo(params.id) then
				if object.GetHealthInfo(params.id).valuePercents then
					dHealthPer = object.GetHealthInfo(params.id).valuePercents
				elseif object.GetHealthInfo(params.id).value and object.GetHealthInfo(params.id).limit then
					dHealthPer = math.ceil(object.GetHealth(params.id).value / object.GetHealth(params.id).limit * 100)
				end
			elseif device.GetShieldStrength(params.id) then
				dHealthPer = math.ceil(device.GetShieldStrength(params.id).value / device.GetShieldStrength(params.id).maxValue * 100)
			end
		end
		if dHealthPer then
			ColorCheck(Device[params.id], dHealthPer)
		end
	end
end

function ShipHealthChanged( params )
	if params.id == ShipId and object.IsExist(params.id) then
		local tHealthPer
		if transport.GetHealth then
			tHealthPer = math.ceil(transport.GetHealth(params.id).value / transport.GetHealth(params.id).limit * 100)
		else
			if object.GetHealthInfo(params.id) then
				if object.GetHealthInfo(params.id).valuePercents then
					tHealthPer = object.GetHealthInfo(params.id).valuePercents
				elseif object.GetHealthInfo(params.id).value and object.GetHealthInfo(params.id).limit then
					tHealthPer = math.ceil(object.GetHealth(params.id).value / object.GetHealth(params.id).limit * 100)
				end
			end
		end
		if tHealthPer then
			ColorCheck(Ship:GetChildChecked("Ship", false), tHealthPer)
		end
	end
end

function ReactorChanged( params )
	if params.id == ShipId then
		local devs = transport.GetDevices(ShipId)
		for j, x in pairs(devs) do
			local devtype = device.GetUsableDeviceType(x)
			if devtype == USDEV_REACTOR then
				ColorCheck(Device[x], math.ceil((transport.GetEnergy(ShipId).limit - transport.GetEnergy(ShipId).value) / transport.GetEnergy(ShipId).limit * 100))
			end
		end
	end
end

function EmanChanged( params )
	if params.id == ShipId then
		Insight:SetVal("name", common.FormatInt(transport.GetInsight(ShipId), "%d"))
	end
end

function SlashCommand( params )
	if common.IsSubstring(params.text, userMods.ToWString("/shuddnd")) then
		local DragPanel = Ship:GetChildChecked("DragPanel", false)
		DragPanel:Show(not DragPanel:IsVisible())
	elseif common.IsSubstring(params.text, userMods.ToWString("/shudtop")) then
		if mainForm:GetPriority() == 10000 then
			mainForm:SetPriority(3000)
		else
			mainForm:SetPriority(10000)
		end
	end
end

function AvatarShip()
	if avatar.GetObservedTransport() then
		local trans = avatar.GetObservedTransport()
		if transport.CanDrawInterface(trans) then
			GetShipData(trans)
		end
	else
		show(m_shipNotFoundHeader)
		Ship:Show(false)
	end
end

function Tooltip(r)
	if r.active then
		for i, v in pairs(Device) do
			if r.widget:IsEqual(v) and object.IsExist(i) then
				local VT = common.CreateValuedText()
				local dHealthPer

				if device.GetUsableDeviceType(i) == USDEV_REACTOR then
					dHealthPer = math.ceil((transport.GetEnergy(ShipId).limit - transport.GetEnergy(ShipId).value) / transport.GetEnergy(ShipId).limit * 100)
				else
					if device.GetHealth then
						dHealthPer = math.ceil(device.GetHealth(i).value / device.GetHealth(i).limit * 100)
					else
						if object.GetHealthInfo(i) then
							if object.GetHealthInfo(i).valuePercents then
								dHealthPer = object.GetHealthInfo(i).valuePercents
							elseif object.GetHealthInfo(i).value and object.GetHealthInfo(i).limit then
								dHealthPer = math.ceil(object.GetHealth(i).value / object.GetHealth(i).limit * 100)
							end
						elseif device.GetShieldStrength(i) then
							dHealthPer = math.ceil(device.GetShieldStrength(i).value / device.GetShieldStrength(i).maxValue * 100)
						end
					end
				end
				if dHealthPer then
				
					local itemId = device.GetItemInstalled( i )
								
						if itemId then
									
							local itemInfo = itemLib.GetItemInfo( itemId )
							local itemQuality = itemLib.GetQuality( itemId )
							local quality = itemQuality and itemQuality.quality
							if itemQuality.quality==ITEM_QUALITY_COMMON then 
									VT:SetFormat(userMods.ToWString("<html fontsize='11' color='0xff00ff00'>"..userMods.FromWString(itemInfo.name)..' '..itemInfo.level.."<br/>"..dHealthPer.."%</html>"))
								elseif  itemQuality.quality==ITEM_QUALITY_UNCOMMON then
									VT:SetFormat(userMods.ToWString("<html fontsize='11' color='0xff1E90FF'>"..userMods.FromWString(itemInfo.name)..' '..itemInfo.level.."<br/>"..dHealthPer.."%</html>"))
								elseif 	itemQuality.quality==ITEM_QUALITY_RARE then
									VT:SetFormat(userMods.ToWString("<html fontsize='11' color='0xffa020f0'>"..userMods.FromWString(itemInfo.name)..' '..itemInfo.level.."<br/>"..dHealthPer.."%</html>"))
								elseif  itemQuality.quality==ITEM_QUALITY_EPIC then
									VT:SetFormat(userMods.ToWString("<html fontsize='11' color='0xffffa500'>"..userMods.FromWString(itemInfo.name)..' '..itemInfo.level.."<br/>"..dHealthPer.."%</html>"))	
							end			
						else
						
							VT:SetFormat(userMods.ToWString("<html fontsize='11'>"..userMods.FromWString(object.GetName(i)).."<br/>"..dHealthPer.."%</html>"))
						
						end

					Ship:GetChildChecked("Tooltip", false):SetVal("name", VT)
					Ship:GetChildChecked("Tooltip", false):Show(true)
				end
				break
			end
		end
		for i, v in pairs(Shield) do
			if r.sender == v:GetName() then
				local side = tonumber(string.sub(r.sender, 11))
				Ship:GetChildChecked("Tooltip", false):SetVal("name", userMods.ToWString(math.ceil(transport.GetShieldStrength(ShipId, side).value / transport.GetShieldStrength(ShipId, side).maxValue * 100).."%"))
				Ship:GetChildChecked("Tooltip", false):Show(true)
				break
			end
		end
	else
		Ship:GetChildChecked("Tooltip", false):Show(false)
	end
end

function ObjHealthChanged(p)
	local transportId = avatar.GetObservedTransport()
	if transportId==nil then return end
	if not object.IsExist(transportId)  then return
	elseif not object.IsExist(p.id)  then return
	elseif object.IsTransport(p.id) then
		ShipHealthChanged(p)
	elseif object.IsDevice(p.id) then
		DeviceHealthChanged(p)
	end
end

function ShipDamage(p)
	local trans = avatar.GetObservedTransport()
	if trans and transport.CanDrawInterface(trans) and p.defender == trans then
		ShipHealthChanged({id = trans})
	end
end

function DeviceSpawn(p)
	local onCurrentShip = false
	local trans = avatar.GetObservedTransport()
	if trans and transport.CanDrawInterface(trans) then
		for i, v in pairs(transport.GetDevices(trans)) do
			if v == p.id then
				onCurrentShip = true
			end
		end
		if onCurrentShip then
			local devType = device.GetUsableDeviceType(p.id)
			if devType ~= USDEV_SCANER then
				local notPlaced = true
				for i, v in pairs(Device) do
					if tonumber(p.id) == tonumber(i) then
						notPlaced = false
					end
				end
				if notPlaced then
					DeviceCreate(p.id)
				end
			end
		end
	end
end

function DeviceDespawn(p)
	if Device[p.id] and not string.find(Device[p.id]:GetName(), "Shield") then
		Device[p.id]:DestroyWidget()
		Device[p.id] = nil
	end
end

function ChestSpawn()
	local trans = avatar.GetObservedTransport()
	if trans and transport.CanDrawInterface(trans) then
		local chests = 0
		for i, v in pairs(avatar.GetDeviceList()) do
			if v and object.GetName(v) then
				local chestname = string.lower(userMods.FromWString(object.GetName(v)))
				for j, x in pairs(L[Loc]) do
					if string.find(chestname, x) then
						chests = chests + 1
					end
				end
			end
		end
		TCount:SetVal("name", common.FormatInt(chests, "%d"))
	end
end

-- AO game Localization detection by SLA. Version 2011-02-05.
function GetGameLocalization()
	local B = cartographer.GetMapBlocks()
	local T = { rus="\203\232\227\224", eng="Holy Land",
	ger="Heiliges Land", fra="Terre Sacr\233e", br="Terra Santa", jpn="\131\74\131\106\131\65" }
	for b in pairs(B) do for l,t in pairs(T) do
	if userMods.FromWString( cartographer.GetMapBlockInfo(B [b] ).name ) == t
	then return l end; end end; return "eng"
end

--ADDON MANAGER COOPERATION
--function OnEventAMMemUsageRequest() --MEMORY
--	userMods.SendEvent( "U_EVENT_ADDON_MEM_USAGE_RESPONSE", { sender = common.GetAddonName(), memUsage = gcinfo() } )
--end

--function OnScriptToggleUI( params ) --UI TOGGLE
--	Ship:Show( params.visible )
--end

function ConfigInitEvent()
	userMods.SendEvent("CONFIG_INIT_EVENT_RESPONSE", { sender = common.GetAddonName() })
end

function ConfigEvent()
	local TL = {
	["eng"] = "Gradual Coloring",
	["rus"] = "Gradual Coloring",
	["br"] = "Gradual Coloring",
	["ger"] = "Gradual Coloring",
	["fra"] = "Gradual Coloring",
	["jpn"] = "Gradual Coloring",
	}
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 0, name = "DnD", btnType = "Simple"})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 1, name = "100%", btnType = "Color", color = SHUD["100%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 2, name = "80%", btnType = "Color", color = SHUD["80%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 3, name = "60%", btnType = "Color", color = SHUD["60%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 4, name = "40%", btnType = "Color", color = SHUD["40%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 5, name = "20%", btnType = "Color", color = SHUD["20%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 6, name = "0%", btnType = "Color", color = SHUD["0%"]})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = 7, name = "Default", btnType = "Simple"})
	userMods.SendEvent("CONFIG_EVENT_RESPONSE", {NoB = "Gradual", name = TL[GetGameLocalization()], btnType = "T/F", state = SHUD["Gradual"]})
end

function ConfigSimple(p)
	if p.name == "DnD" then
		local DragPanel = Ship:GetChildChecked("DragPanel", false)
		DragPanel:Show(not DragPanel:IsVisible())
	else
		ColorSet()
		userMods.SendEvent("CONFIG_CLEAR_BUTTONS", {})
		ConfigEvent()
	end
end

function ConfigColor(p)
	SHUD[p.name] = p.color
	userMods.SetGlobalConfigSection("TargetSHUD", SHUD)
end

function ColorSet()
	SHUD["100%"] = {r = 0.09; g = 0.52; b = 0.95; a = .8}
	SHUD["80%"] = {r = 0.08; g = 0.64; b = 0.18; a = .8}
	SHUD["60%"] = {r = .9; g = .9; b = 0; a = .8}
	SHUD["40%"] = {r = .8; g = 0.35; b = 0.02; a = .8}
	SHUD["20%"] = {r = .9; g = 0.2; b = 0; a = .8}
	SHUD["0%"] = {r = .3; g = .3; b = .3; a = .8}
	userMods.SetGlobalConfigSection("TargetSHUD", SHUD)
end

function ConfigTF(p)
	SHUD["Gradual"] = p.state
	userMods.SetGlobalConfigSection("TargetSHUD", SHUD)
end

function ChangeDeviceWndVisible()
	if not ShipContainer:IsVisible() then
		show(ShipContainer)
	else
		hide(ShipContainer)
	end
end
--------------------------------------------------------------------------------
-- INITIALIZATION
--------------------------------------------------------------------------------
function Init()
	if not g_showDevices then
		hide(ShipPanel)
		return
	end 
	if not SHUD then
		SHUD = {}
		ColorSet()
	end
	if SHUD["Gradual"] == nil then
		SHUD["Gradual"] = true
		userMods.SetGlobalConfigSection("TargetSHUD", SHUD)
	end
	common.RegisterEventHandler( AvatarShip, "EVENT_TRANSPORT_OBSERVING_STARTED" )
	common.RegisterEventHandler( AvatarShip, "EVENT_TRANSPORT_OBSERVING_FINISHED" )
	common.RegisterEventHandler( DeviceHealthChanged, "EVENT_OBJECT_HEALTH_CHANGED" )
	common.RegisterEventHandler( DeviceHealthChanged, "EVENT_TRANSPORT_SHIELD_CHANGED" )
	common.RegisterEventHandler( DeviceHealthChanged, "EVENT_SHIELD_STRENGTH_CHANGED" )
	common.RegisterEventHandler( ShipHealthChanged, "EVENT_OBJECT_HEALTH_CHANGED" )
	common.RegisterEventHandler( ObjHealthChanged, "EVENT_OBJECT_HEALTH_CHANGED" )
	common.RegisterEventHandler( SlashCommand, "EVENT_UNKNOWN_SLASH_COMMAND")
	common.RegisterEventHandler( ReactorChanged, "EVENT_TRANSPORT_ENERGY_CHANGED" )
	common.RegisterEventHandler( EmanChanged, "EVENT_TRANSPORT_INSIGHT_CHANGED" )
	common.RegisterEventHandler( ShipDamage, "EVENT_SHIP_DAMAGE_RECEIVED" )
	common.RegisterEventHandler( ChestSpawn, "EVENT_DEVICE_DESPAWNED")
	common.RegisterEventHandler( ChestSpawn, "EVENT_DEVICE_SPAWNED")
	common.RegisterEventHandler( DeviceSpawn, "EVENT_USABLE_DEVICE_SPAWNED")
	common.RegisterEventHandler( DeviceDespawn, "EVENT_USABLE_DEVICE_DESPAWNED")
	
	common.RegisterEventHandler( ConfigInitEvent, "CONFIG_INIT_EVENT" )
	common.RegisterEventHandler( ConfigEvent, "CONFIG_EVENT_"..common.GetAddonName())
	common.RegisterEventHandler( ConfigColor, "CONFIG_COLOR_"..common.GetAddonName())
	common.RegisterEventHandler( ConfigSimple, "CONFIG_SIMPLE_"..common.GetAddonName())
	common.RegisterEventHandler( ConfigTF, "CONFIG_BUTTON_"..common.GetAddonName())
	
	common.RegisterReactionHandler( Tooltip, "Tooltip" )
	Loc = GetGameLocalization()
	local DragPanel = Ship:GetChildChecked("DragPanel", false)
	DragPanel:SetBackgroundColor({r=0;g=0;b=0;a=0})
	DragPanel:Show(false)
	mainForm:SetPriority(10000)
	
	TooltipText:SetPriority(10)
	local template = createWidget(nil, "Template", "Template")
	setTemplateWidget(template)
	local btn = createWidget(ShipPanel, "DevicesButton", "Button", WIDGET_ALIGN_LOW, WIDGET_ALIGN_LOW, 30, 25, 0, 0)
	setText(btn, "ND")
	btn:SetPriority(0)
	AddReaction("DevicesButton", function () ChangeDeviceWndVisible() end)
	m_shipNameHeader = createWidget(Ship, "txtHeader", "TextView", nil, nil, 160, 25, 0, 0)
	m_shipNotFoundHeader = createWidget(ShipContainer, "noShipHeader", "TextView", nil, nil, 160, 25, 0, 0)
	setText(m_shipNotFoundHeader, "Нет цели", "ColorYellow", "center")
	
	
	DnD:Init( ShipPanel, btn, true)
	AvatarShip()
end
--------------------------------------------------------------------------------
common.RegisterEventHandler( Init, "EVENT_AVATAR_CREATED" )
if avatar.IsExist() then
	Init()
end
--------------------------------------------------------------------------------