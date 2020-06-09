-- Panel of loot

local wtLoot = mainForm:GetChildChecked("LogPanel", true)
wtLoot:SetClipContent(false)
wtLoot:SetSmartPlacementPlain({posY = 600})

LLimC.Init( wtLoot, nil, nil, -15 )

Global("gLogBuffer", {})
Global("gLogTimer", nil)
Global("gLogTimerStarted", false)

function PutItem(itemObject)
	local itemValuedObject = itemObject
	local itemId = itemValuedObject:GetId()
	local itemQuality = itemLib.GetQuality( itemId ).quality
	if itemQuality < ITEM_QUALITY_COMMON then return false end
	local itemCount = itemLib.GetStackInfo( itemId ).count
	for i, v in pairs(gLogBuffer) do
		if avatar.IsItemsStackable( v.itemId, itemId ) then
			gLogBuffer[i].itemCount = gLogBuffer[i].itemCount + itemCount
			return true
		end
	end
	table.insert(gLogBuffer, {itemId = itemId, itemCount = itemCount, itemValuedObject = itemValuedObject})
	return true
end

function OnLootReceive(params)
	if not PutItem(params.itemObject) then return end
	if not gLogTimerStarted then
		gLogTimerStarted = true
		StartTimer(gLogTimer, true)
	end
end

function OnLogTimer()
	for _, v in pairs(gLogBuffer) do
		FillLogPanel(v)
	end
	gLogTimerStarted = false
	gLogBuffer = nil
	gLogBuffer = {}
end

function FillLogPanel(itemObject)
	local itemId = itemObject.itemId
	local itemValuedObject = itemObject.itemValuedObject
	local itemQuality = itemLib.GetQuality( itemId ).quality
	--if itemQuality < ITEM_QUALITY_COMMON then return end
	local itemCount = itemObject.itemCount
	local wtBackground = mainForm:CreateWidgetByDesc(wtLoot:GetChildChecked("Background", true):GetWidgetDesc())
	--wtBackground:SetSmartPlacementPlain({alignX = ENUM_AlignX_CENTER})
	local wtText = wtBackground:GetChildChecked("Text", true)
	local wtIcon = wtBackground:GetChildChecked("Icon", true)
	
	--wtBackground:SetSmartPlacementPlain({sizeY = 50, sizeX = 450})
	
	--wtText:SetSmartPlacementPlain({highPosY = 10, posY = 5, sizeX = 285})
	--wtText:SetSmartPlacementPlain({posY = 9, highPosY = 14})
	--wtIcon:SetSmartPlacementPlain({posY = -3, highPosY = 0, alignY = ENUM_AlignY_MIDDLE})
	--wtIcon:SetSmartPlacementPlain({highPosY = 18})
	
	wtBackground:SetBackgroundColor( { r = 1.0; g = 1.0; b = 1.0; a = 0.6 } )
	
	wtIcon:SetBackgroundTexture(itemValuedObject:GetImage())
	wtText:Show(true)
	--wtText:SetMultiline(true)
	--[[
	if params.playerId ~= nil then		
		common.SetTextValues(wtText, {
		format = userMods.ToWString("<body alignx='left'  fontsize='20' shadow='1' outline='1' outlinecolor=\"0x000000\"><rs class='playerColor'><r name='playerName'/></rs> <tip_white><r name='locText'/> <rs class='itemColor'>[<r name='itemText'/>]</rs><r name='_itemCount'/></tip_white></body>"),
		locText = userMods.ToWString("получает"),
		itemText = itemValuedObject,
		itemColor = GetItemCSS(itemId),
		_itemCount = userMods.ToWString(itemCount > 1 and " <br/>"..itemCount or ""),
		playerName = object.GetName(params.playerId),
		playerColor = raid.IsExist() and "Raid" or "tip_blue"
		})
	else
	]]--
	common.SetTextValues(wtText, {
	format = userMods.ToWString('<header alignx="center" fontsize="16" shadow="1"><rs class="itemColor"><r name="itemText"/></rs><tip_white><br/><r name="_itemCount"/></tip_white></header>'),
	itemText = itemValuedObject:GetText(),
	_itemCount = userMods.ToWString(itemCount > 1 and ""..itemCount or " "),
	itemColor = GetItemCSS(itemId)
	})
	wtIcon:Show(true)
	LLimC.Push(wtLoot, wtBackground)
end

common.RegisterEventHandler( OnLootReceive, "EVENT_AVATAR_ITEM_TAKEN")
--common.RegisterEventHandler( OnLootReceive, "EVENT_AVATAR_ITEM_TAKEN_BY_GROUPMATE", {playerId = nil})
--[[


--for i, v in pairs(itemIds) do
	--Chat(" -> "..userMods.FromWString(itemLib.GetItemInfo(containerLib.GetItems( ITEM_CONT_INVENTORY )[0]).name))
--end
if avatar.IsItemsStackable( containerLib.GetItem( ITEM_CONT_INVENTORY, 0 ), containerLib.GetItem( ITEM_CONT_INVENTORY, 1 ) ) then
	Chat("Same")
end
function PlayAnim(form)
	if form == nil then return end
	for _, v in pairs(form:GetNamedChildren()) do
		--v:Show(false)
		--v:PlayBackground( false )
		PlayAnim(v)
	end
end
PlayAnim(stateMainForm)
]]--
	--common.StateUnloadManagedAddon( "MedalsAnnouncer" )
	--common.StateLoadManagedAddon( "MedalsAnnouncer" )
--common.GetAddonMainForm( "MedalsAnnouncer" ):GetChildChecked("Announce", true):GetChildChecked("Control3D", true):PlayBackground( true )
--common.GetAddonMainForm( "MedalsAnnouncer" ):GetChildChecked("MovedIcon", true):PlayBackground( true )


--local mFormInfo = mform:GetInfo()
--for i, v in pairs(mform:GetInfo()) do
--	Chat(i)
--end

--[[
for i, v in pairs(common.GetCSSList()) do
	LogInfo(v)
end
]]--

gLogTimer = InitTimer(OnLogTimer, 100, false, nil)