-- WIDGETS
Global( "wtMainPanel", nil)
Global( "wtSettingsPanel", nil)
Global( "wtItem", nil)
Global( "wtSeparator", nil)
Global( "wtButton", nil)
Global( "wtContainer", nil)
Global( "wtAddonName", nil)
Global( "wtCheckbox", mainForm:GetChildChecked("Checkbox", true))
Global( "wtUp", mainForm:GetChildChecked("Up", true))
Global( "wtHide", mainForm:GetChildChecked("ShowHideBtn", false))

wtUp:Show(false)

Global( "wtInfoText", mainForm:GetChildChecked("InfoPanelText", true))
Global( "wtInfoPanel", mainForm:GetChildChecked("InfoPanel", true))

Global( "wtButtonTurn", mainForm:GetChildChecked("ButtonTurn", true))
Global( "wtButtonDeposite", mainForm:GetChildChecked("ButtonDeposite", true))

--wtButtonDeposite:GetChildChecked("Lighting", true):SetBackgroundColor({a = 0.7, r = 1.0, g = 1.0, b = 1})

wtInfoPanel:Show(false)

Global( "wtDays", nil)
Global( "wtDaysBag", nil)

Global( "gOffset", 0)

Global( "gCategories", {TXT("Items"), TXT("Crafting"), TXT("Boutique")})
Global( "gDays", 1)
Global( "gDaysBag", 3)

Global( "gDaysVariants", {3, 7, 21, 30, 180, 0})

Global( "gThings", 1)
Global( "gCrafts", 1)
Global( "gRarities", 1)

Global( "gPocket", -1)

Global( "gMode", 0)

Global( "gShow", false)

Global( "gPointedSlot", nil)

Global( "gTimer", nil)

Global( "gHideMode", 0)

Global( "wtItemWidgets", {})

Global( "wtTextItem", {})

Global( "wtCategories", {})

Global( "wtSeparators", {})
Global( "wtItems", {})

Global( "plItemNull", nil)
Global( "plItemNormal", nil)

Global("gTimerInventoryChangedInventory", nil )
Global("gTimerInventoryChangedDeposite", nil )

Global( "COLORS", {
[ITEM_QUALITY_JUNK] = "Junk",
[ITEM_QUALITY_GOODS] = "Goods",
[ITEM_QUALITY_COMMON] = "Common",
[ITEM_QUALITY_UNCOMMON] = "Uncommon",
[ITEM_QUALITY_RARE] = "Rare",
[ITEM_QUALITY_EPIC] = "Epic",
[ITEM_QUALITY_LEGENDARY] = "Legendary",
[ITEM_QUALITY_RELIC] = "Relic",
[ITEM_QUALITY_DRAGON] = "Dragon"
})

function GetItemCSS(itemId)
	local css = COLORS[itemLib.GetQuality(itemId).quality]
	if itemLib.IsCursed( itemId ) then
		css = css.."Cursed"
	end
	return css
end

local wtTab01 = common.GetAddonMainForm( "ContextBag" ):GetChildChecked("Tab01", true)
local wtTab02 = common.GetAddonMainForm( "ContextBag" ):GetChildChecked("Tab02", true)
local wtTab03 = common.GetAddonMainForm( "ContextBag" ):GetChildChecked("Tab03", true)
local wtBag = 	common.GetAddonMainForm( "ContextBag" ):GetChildChecked("Bag", true)
local wtBagArea = 	common.GetAddonMainForm( "ContextBag" ):GetChildChecked("Bag", true):GetChildChecked("Area", true)

local wtDepositeBox = 	common.GetAddonMainForm( "ContextDepositeBox" ):GetChildChecked("MainPanel", true)
--------------------------------------------------------------------------------
-- EVENT HANDLERS
--------------------------------------------------------------------------------

function ShowPanel(bool)
	if not bool then
		wtMainPanel:Show(false)
		wtSettingsPanel:Show(false)
	else
		wtMainPanel:Show(true)
		if gMode == 1 then
			FillPanel(ITEM_CONT_DEPOSITE)
		else		
			FillPanel(ITEM_CONT_INVENTORY)
		end
	end
end

function ShowHideBtnReaction(params)
	if params.widget:GetParent():GetName() == "SettingsPanel" then
		wtSettingsPanel:Show(false)
		return
	end
	if DnD.IsDragging() then return end
	ShowPanel(not wtMainPanel:IsVisible())
end

function OnFormEffect(params)
	--UpdateBase(params)
	if wtMainPanel:IsVisible() then
		FillPanel(gMode == 0 and ITEM_CONT_INVENTORY or ITEM_CONT_DEPOSITE)
	end
	gPocket = -1
end

function OnInventorySizeChanged(params)
	ResetTextItem()
end

function DestroyItems()
	gOffset = wtContainer:GetContainerOffset()
	wtItemWidgets = {}
	for _, z in pairs(wtSeparators) do
		wtContainer:RemoveAt(z)
	end
	wtSeparators = {}
end
--------------------------------------------------------------------------------
-- FUNCTIONS
--------------------------------------------------------------------------------
function GetActiveTab()
	if wtTab01:GetVariant() == 1 then
		return 0
	end
	if wtTab02:GetVariant() == 1 then
		return 1
	end
	if wtTab03:GetVariant() == 1 then
		return 2
	end
	return nil
end

function ResetTextItem()
	for _, v in pairs(wtTextItem) do
		v:DestroyWidget()
		v = nil
	end
	local slotLine, item = 0, 1
	for i = 0, avatar.GetInventorySize() / 3 - 1 do
		if i % 6 == 0 then
			slotLine = slotLine + 1
			item = 1
		end
		local txt = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("TextItem", true):GetWidgetDesc())
		wtBagArea:GetChildChecked("SlotLine"..userMods.FromWString(common.FormatInt( slotLine, "%02d" )), true):GetChildChecked("Item"..userMods.FromWString(common.FormatInt( item, "%02d" )), true):GetChildChecked("Frame", true):AddChild(txt)		
		wtBagArea:GetChildChecked("SlotLine"..userMods.FromWString(common.FormatInt( slotLine, "%02d" )), true):GetChildChecked("Item"..userMods.FromWString(common.FormatInt( item, "%02d" )), true):GetChildChecked("Frame", true):AddChild(wtBagArea:GetChildChecked("SlotLine"..userMods.FromWString(common.FormatInt( slotLine, "%02d" )), true):GetChildChecked("Item"..userMods.FromWString(common.FormatInt( item, "%02d" )), true):GetChildChecked("Link", true))
		wtTextItem[i] = txt
		item = item + 1
	end
end

function RefreshItemTime()
	if wtBag:IsVisible() == false then
		return
	end
	local activeTab = GetActiveTab()
	if gPocket == activeTab or activeTab == nil then
		return
	end
	for i = 0, avatar.GetInventorySize() / 3 - 1 do
		local _i = i + avatar.GetInventorySize() / 3 * activeTab
		wtTextItem[i]:Show(false)
		if avatar.GetInventoryItemId( _i ) then
			local itemTInfo = itemLib.GetTemporaryInfo( avatar.GetInventoryItemId( _i ) )
			if itemTInfo and math.floor(itemTInfo.remainingMs/ 60/ 60/ 24/ 1000) <= gDaysVariants[gDaysBag] then
				common.SetTextValues( wtTextItem[i], {val = userMods.ToWString(FromMillisecondsToString(itemTInfo.remainingMs, true))})
				wtTextItem[i]:Show(true)
			end
		end
	end
	gPocket = activeTab
end

function ClearItemTime()
	for i = 0, avatar.GetInventorySize() / 3 - 1 do
		wtTextItem[i]:Show(false)
	end
end

function FromMillisecondsToString(mill, isItemText, todayMs)
	local FullSecond = (mill) / 1000
	if not isItemText and todayMs then
		FullSecond = FullSecond - todayMs / 1000
	end
	if FullSecond <= 0 then
		return "˜˜˜˜˜˜˜"
	end
	local days =  math.floor(FullSecond/ 60/ 60/ 24)
	FullSecond = FullSecond - days * 86400
	local hours = math.floor(FullSecond/ 60/ 60)
	FullSecond = FullSecond - hours * 60*60
	local minutes = math.floor(FullSecond/ 60)
	FullSecond = FullSecond - minutes * 60
	local seconds = math.floor(FullSecond)
	return isItemText and (days > 0 and days..TXT("Day") or (hours > 0 and hours..TXT("Hour") or (minutes > 0 and TXT("LessThanHour") or TXT("LessThanMinute")))) or (days > 0 and days..TXT("Day").." " or "")..(hours > 0 and hours..TXT("Hour").." " or "")..(minutes > 0 and minutes..TXT("Minute").." " or "")
end

function Comparison(a, b)
	if(a.pocket < b.pocket) then
		return true
	elseif(a.pocket > b.pocket) then
		return false
	else
		if(a.temporary < b.temporary) then
			return true
		else
			return false
		end		
	end
end

function CheckItem(itemId, container)
	if itemId == nil then
		return false
	end
	local temporaryInfo = itemLib.GetTemporaryInfo( itemId )
	if temporaryInfo == nil then
		return false
	end
	if container == ITEM_CONT_DEPOSITE then
		return true
	end
	local pocket = avatar.InventoryGetItemPocket( itemId )
	if pocket == -1 and gThings ~= 1 or pocket == 0 and gCrafts ~= 1 or pocket == 1 and gRarities ~= 1 then
		return false
	end
	return true
end

Global("gMaxItem", 0)
Global("gBase", {})
gBase[ITEM_CONT_DEPOSITE] = {}
gBase[ITEM_CONT_INVENTORY] = {}

function FillPanel(container)
	DestroyItems()
	if table.maxn(gBase[container]) == 0 then
		wtMainPanel:GetChildChecked("TextEmpty", true):Show(true)
		for i = 1, gMaxItem do
			wtItems[i]:SetPlacementPlain(plItemNull)
		end
		gMaxItem = 1
		return
	else
		wtMainPanel:GetChildChecked("TextEmpty", true):Show(false)	
	end
	wtSeparator:Show(false)
	local gCategory = 2
	local index = 1
	if container == ITEM_CONT_DEPOSITE then
		local _wtSeparator = mainForm:CreateWidgetByDesc(wtSeparator:GetWidgetDesc())
		wtContainer:Insert(index - 1, _wtSeparator)
		table.insert(wtSeparators, 0, 0)
		common.SetTextValues( _wtSeparator:GetChildChecked("SeparatorText", true), {val = userMods.ToWString(TXT("Bank"))})
	end	
    local separatorOffset = 0
    
	for _, v in pairs(gBase[container]) do
		if v.pocket ~= gCategory and container == ITEM_CONT_INVENTORY then
			local _wtSeparator = mainForm:CreateWidgetByDesc(wtSeparator:GetWidgetDesc())
			wtContainer:Insert(index - 1 + separatorOffset, _wtSeparator)
			table.insert(wtSeparators, 0, index - 1 + separatorOffset)
			separatorOffset = separatorOffset + 1
			common.SetTextValues( _wtSeparator:GetChildChecked("SeparatorText", true), {val = userMods.ToWString(gCategories[v.pocket + 2])})
			gCategory = v.pocket
        end
        
		local _wtItem = wtItems[index]
		_wtItem:SetPlacementPlain(plItemNormal)
		_wtItem:Show(true)
        local wtIcon = _wtItem:GetChildChecked("Button", true)
        
		wtItemWidgets[wtIcon:GetInstanceId()] = v
        wtIcon:SetBackgroundTexture(v.icon)
        
		if 	container ~= ITEM_CONT_DEPOSITE and
			itemLib.IsUsable( v.id ) == false and
			itemLib.GetBoxInfo( v.id ) == nil and
			itemLib.IsUseItemAndTakeActions( v.id ) == false and
			itemLib.CanActivateForUseItem( v.id ) == false then
				wtIcon:Enable(false)
		else
			wtIcon:Enable(true)
        end
        
		local wtCount = wtIcon:GetChildChecked("Count", true)	
        common.SetTextValues( wtCount, {val = common.FormatInt(v.count , "%dK5" )})
        
		local wtName = _wtItem:GetChildChecked("Name", true)
        common.SetTextValues( wtName, {val = v.name, class = GetItemCSS(v.id)})
        
		local wtTemporary = _wtItem:GetChildChecked("Time", true)
		common.SetTextValues( wtTemporary, {val = userMods.ToWString(FromMillisecondsToString(v.temporary, false, mission.GetGlobalDateTime().overallMs))})
		index = index + 1
	end
	for i = index, gMaxItem do
		wtItems[i]:SetPlacementPlain(plItemNull)
	end
	gMaxItem = index
	wtContainer:SetContainerOffset(gOffset)
end

function OnPressButtonDown(params)
	local v = wtItemWidgets[params.widget:GetInstanceId()]
	if v == nil then return end 
	v = v.id
	if itemLib.IsItem( v ) == false then return end
	if avatar.CheckCanUseItem( v, false ) == false then return end
	if containerLib.GetItemSlot( v ).slotType == ITEM_CONT_DEPOSITE then
		containerLib.MoveItem( v, ITEM_CONT_INVENTORY, nil, nil )
		return
	end
	if avatar.IsInspectAllowed() == false then
		return
	end
	if itemLib.IsUsable( v ) then
		avatar.UseItem( v )
		return
	end
	if itemLib.GetBoxInfo( v ) ~= nil then
		avatar.OpenBox( v )
		return
	end
	if itemLib.IsUseItemAndTakeActions( v ) then
		avatar.UseItemAndTakeActions( v )
		return
	end
	if itemLib.CanActivateForUseItem( v ) then
		avatar.UseItemAndTakeActions( v )
		return
	end
end

function OnPointingButton(params)
	if gPointedSlot ~= nil then
		gPointedSlot:PlayFadeEffect( 0.5, 0.0, 200, EA_MONOTONOUS_INCREASE )
		gPointedSlot = nil
		return
	end
	if wtItemWidgets[params.widget:GetInstanceId()] == nil then
		return
	end
	if itemLib.IsItem( wtItemWidgets[params.widget:GetInstanceId()].id ) == false then return end
	local tble = containerLib.GetItemSlot( wtItemWidgets[params.widget:GetInstanceId()].id )
	
	if tble == nil then return end
	if tble.slotType == ITEM_CONT_DEPOSITE then	return end
	
	local v = wtItemWidgets[params.widget:GetInstanceId()].index
	local slotId = v % (avatar.GetInventorySize() / 3)
	local pocketNum = math.floor(v / (avatar.GetInventorySize() / 3))
	if pocketNum ~= GetActiveTab() then
		return
	end
	
	local slotLine = common.FormatInt( 1 + math.floor(slotId / 6), "%02d" )
	local item = 1 + slotId - math.floor(slotId / 6) * 6
	
	gPointedSlot = wtBagArea:GetChildChecked("SlotLine"..userMods.FromWString(slotLine), true):GetChildChecked("Item"..userMods.FromWString(common.FormatInt( item, "%02d" )), true):GetChildChecked("Blink", true)
	gPointedSlot:Show( true )
	gPointedSlot:PlayFadeEffect( 0.5, 1.0, 500, EA_MONOTONOUS_INCREASE )
end

function CreateSettings()
	wtSettingsPanel =  mainForm:CreateWidgetByDesc(wtMainPanel:GetWidgetDesc())
	DnD.Init( wtSettingsPanel, nil, false)
	wtSettingsPanel:SetName("SettingsPanel")
	wtSettingsPanel:GetChildChecked("ButtonSettings", true):DestroyWidget()
	wtSettingsPanel:GetChildChecked("ButtonTurn", true):DestroyWidget()
	wtSettingsPanel:GetChildChecked("ButtonRefresh", true):DestroyWidget()
	wtSettingsPanel:GetChildChecked("ButtonDeposite", true):DestroyWidget()
	wtSettingsPanel:GetChildChecked("Container", true):GetChildChecked("Item", true):Show(false)
	wtSettingsPanel:GetChildChecked("Button", true):Show(false)
	wtSettingsPanel:GetChildChecked("Separator", true):Show(false)
	
	local wtSettingsCorner = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())
	local pl = wtSettingsCorner:GetPlacementPlain()
	wtSettingsCorner:SetVal("val", TXT("Settings"))
	pl.alignX = 3
	pl.alignY = 0
	pl.posY = 10
	wtSettingsCorner:	SetFormat("<header alignx = \"center\" fontsize=\"18\"><rs class=\"class\"><r name=\"val\"/></rs></header>")
	wtSettingsCorner:SetPlacementPlain(pl)
	wtSettingsCorner:Show(true)
	wtSettingsPanel:AddChild(wtSettingsCorner)
	
	local _wtSeparator = mainForm:CreateWidgetByDesc(wtSeparator:GetWidgetDesc())
	_wtSeparator:SetName("SettingsDisplay")
	wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtSeparator)
	_wtSeparator:Show(true)
	_wtSeparator:GetChildChecked("SeparatorText", true):SetVal("val", TXT("SettingsDisplay"))
				
	for _, v in pairs(gCategories) do
		local _wtItem = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Item", true):GetWidgetDesc())
		_wtItem:GetChildChecked("Button", true):DestroyWidget()
		_wtItem:SetBackgroundColor({a = 0.0, r = 0, g = 0, b = 0})
		_wtItem:Show(true)	
		
		pl = _wtItem:GetPlacementPlain()
		pl.sizeY = pl.sizeY - 15
		_wtItem:SetPlacementPlain(pl)
		
		local _wtCheckbox = mainForm:CreateWidgetByDesc(wtCheckbox:GetWidgetDesc())
		wtCategories[v] = _wtCheckbox
		pl = _wtCheckbox:GetPlacementPlain()
		pl.posX = 17
		pl.posY = 5
		_wtCheckbox:SetPlacementPlain(pl)
		_wtCheckbox:Show(true)
		_wtCheckbox:SetName(v)
		_wtItem:AddChild(_wtCheckbox)
		
		local wtText = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())		
		
		wtText:SetFormat("<header alignx=\"left\" fontsize=\"14\"><html outline=\"1\" shadow=\"1\" outlinecolor=\"0x000000\"><b><r name=\"val1\"/> \"<r name=\"val2\"/>\"</b></html></header>")
		
		wtText:SetVal("val1", userMods.ToWString(TXT("Settings1")))
		wtText:SetVal("val2", userMods.ToWString(v))
		
		pl = wtText:GetPlacementPlain()
		pl.posX = 46+5
		pl.posY = 5
		wtText:SetPlacementPlain(pl)
		wtText:Show(true)		
		_wtItem:AddChild(wtText)
		
		wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtItem)
	end
	
	_wtSeparator = mainForm:CreateWidgetByDesc(wtSeparator:GetWidgetDesc())
	_wtSeparator:SetName("SettingsNotif")
	wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtSeparator)
	_wtSeparator:Show(true)
	_wtSeparator:GetChildChecked("SeparatorText", true):SetVal("val", TXT("SettingsNotif"))
	
	local _wtItem = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Item", true):GetWidgetDesc())
	_wtItem:SetBackgroundColor({a = 0.0, r = 0, g = 0, b = 0})
	_wtItem:Show(true)	
	_wtItem:GetChildChecked("Button", true):DestroyWidget()

	pl = _wtItem:GetPlacementPlain()
	pl.sizeY = pl.sizeY-5
	_wtItem:SetPlacementPlain(pl)

	local wtText = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())		
	wtText:SetFormat("<header  alignx = \"left\" fontsize=\"14\"><rs class=\"class\"><html outline=\"1\" shadow=\"1\" outlinecolor=\"0x000000\"><b> <r name=\"val1\"/></b></html></rs></header>")
	wtText:SetVal("val1", userMods.ToWString(TXT("Settings2")))
	wtText:SetMultiline( true )
	wtText:SetWrapText( true )
	pl = wtText:GetPlacementPlain()
	pl.posX = 17
	pl.posY = 4
	pl.sizeX = 250
	pl.sizeY = 50
	wtText:SetPlacementPlain(pl)
	wtText:Show(true)		
	_wtItem:AddChild(wtText)

	wtDays = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())		
	wtDays:SetFormat("<header  alignx = \"center\" fontsize=\"20\"><rs class=\"class\"><html outline=\"1\" shadow=\"1\" outlinecolor=\"0x000000\"><b><r name=\"val\"/></b></html></rs></header>")
	wtDays:SetVal("val", tostring(gDaysVariants[gDays]))
	pl = wtDays:GetPlacementPlain()
	pl.posX = 160
	pl.posY = 6
	wtDays:SetPlacementPlain(pl)
	wtDays:Show(true)		
	_wtItem:AddChild(wtDays)
	
	local _wtUp = mainForm:CreateWidgetByDesc(wtUp:GetWidgetDesc())
	_wtUp:SetName("DayUp")
	pl = _wtUp:GetPlacementPlain()
	pl.posX = 355
	pl.posY = 8
	pl.alignX = 0
	pl.alignY = 0
	_wtUp:SetPlacementPlain(pl)
	_wtUp:Show(true)
	_wtItem:AddChild(_wtUp)

    wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtItem)
    
	_wtSeparator = mainForm:CreateWidgetByDesc(wtSeparator:GetWidgetDesc())
	_wtSeparator:SetName("SettingsBag")
	wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtSeparator)
	_wtSeparator:Show(true)
	_wtSeparator:GetChildChecked("SeparatorText", true):SetVal("val", TXT("SettingsBag"))
	
	local _wtItem = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Item", true):GetWidgetDesc())
	_wtItem:SetBackgroundColor({a = 0.0, r = 0, g = 0, b = 0})
	_wtItem:Show(true)	
	_wtItem:GetChildChecked("Button", true):DestroyWidget()

	pl = _wtItem:GetPlacementPlain()
	pl.sizeY = pl.sizeY + 15
	_wtItem:SetPlacementPlain(pl)

	local wtText = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())		
	wtText:SetFormat("<header  alignx = \"left\" fontsize=\"14\"><rs class=\"class\"><html outline=\"1\" shadow=\"1\" outlinecolor=\"0x000000\"><b> <r name=\"val\"/></b></html></rs></header>")
	wtText:SetVal("val", userMods.ToWString(TXT("Settings3")))
	wtText:SetMultiline( true )
	wtText:SetWrapText( true )
	pl = wtText:GetPlacementPlain()
	pl.posX = 17
	pl.posY = 4
	pl.sizeX = 250
	pl.sizeY = 100
	wtText:SetPlacementPlain(pl)
	wtText:Show(true)		
	_wtItem:AddChild(wtText)

	wtDaysBag = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())		
	wtDaysBag:SetFormat("<header  alignx = \"center\" fontsize=\"20\"><rs class=\"class\"><html outline=\"1\" shadow=\"1\" outlinecolor=\"0x000000\"><b><r name=\"val\"/></b></html></rs></header>")
	wtDaysBag:SetVal("val", tostring(gDaysVariants[gDaysBag]))
	pl = wtDaysBag:GetPlacementPlain()
	pl.posX = 160
	pl.posY = 6 + 5
	wtDaysBag:SetPlacementPlain(pl)
	wtDaysBag:Show(true)		
	_wtItem:AddChild(wtDaysBag)
	
	local _wtUp = mainForm:CreateWidgetByDesc(wtUp:GetWidgetDesc())
	_wtUp:SetName("DayBagUp")
	pl = _wtUp:GetPlacementPlain()
	pl.posX = 355
	pl.posY = 8 + 5
	pl.alignX = 0
	pl.alignY = 0
	_wtUp:SetPlacementPlain(pl)
	_wtUp:Show(true)
	_wtItem:AddChild(_wtUp)

	wtSettingsPanel:GetChildChecked("Container", true):PushBack(_wtItem)
	
end

function OnUpPress(params)
	if params.sender == "DayUp" then
		gDays = gDays == table.maxn(gDaysVariants) and 1 or gDays + 1
		common.SetTextValues(wtDays, {val = userMods.ToWString(tostring(gDaysVariants[gDays]))})
	elseif params.sender == "DayBagUp" then
		gDaysBag = gDaysBag == table.maxn(gDaysVariants) and 1 or gDaysBag + 1
		common.SetTextValues(wtDaysBag, {val = userMods.ToWString(tostring(gDaysVariants[gDaysBag]))})
	end
	SaveConfig()
end

function OnSettingsPress(params)
	wtSettingsPanel:Show(not wtSettingsPanel:IsVisible())
	if wtSettingsPanel:IsVisible() then
		local posConverter = widgetsSystem:GetPosConverterParams()
		local pl1, pl2 = wtMainPanel:GetPlacementPlain(), wtSettingsPanel:GetPlacementPlain()
		pl2.posX = (pl1.posX - pl1.sizeX <= 0) and pl1.posX + pl1.sizeX or pl1.posX - pl1.sizeX
		pl2.posY = pl1.posY
		wtSettingsPanel:SetPlacementPlain(pl2)
	end		
end

function OnCheckboxPress(params)
	params.widget:SetVariant(1 - params.widget:GetVariant())
	SaveConfig()
end

function OnSeparatorPointing(params)
	if params.active == false then
		wtInfoPanel:Show(false)
		return
	end
	if params.sender == "SettingsDisplay" then
		wtInfoText:SetFormat(TXT("SettingsDisplayHelp"))
		wtInfoPanel:Show(true)
	elseif params.sender == "SettingsNotif" then
		wtInfoText:SetFormat(TXT("SettingsNotifHelp"))
		wtInfoPanel:Show(true)
	elseif params.sender == "SettingsBag" then
		wtInfoText:SetFormat(TXT("SettingsBagHelp"))
		wtInfoPanel:Show(true)
	end
	local pl = wtInfoPanel:GetPlacementPlain()
	local plInfoPanel = params.widget:GetParent():GetParent():GetParent():GetParent():GetPlacementPlain()
	pl.posX = plInfoPanel.posX + plInfoPanel.sizeX
	pl.posY = plInfoPanel.posY
	wtInfoPanel:SetPlacementPlain(pl)
end

function LoadConfig()
	local loadConfig = userMods.GetGlobalConfigSection( "Settings" )
	if loadConfig ~= nil then
		gThings = loadConfig.gThings or 1
		gCrafts = loadConfig.gCrafts or 1
		gRarities = loadConfig.gRarities or 1
		
		gDays = loadConfig.gDays or 1
		gDaysBag = loadConfig.gDaysBag or 3
		gShow = loadConfig.gShow or false
		gHideMode = loadConfig.gHideMode or 0
	end
	
	wtCategories[gCategories[1]]:SetVariant(gThings)
	wtCategories[gCategories[2]]:SetVariant(gCrafts)
	wtCategories[gCategories[3]]:SetVariant(gRarities)
	
	wtHide:Show(not gShow)
	
	wtButtonTurn:GetChildChecked("Lighting", true):Show(gShow)
	
	wtHide:SetVariant(gHideMode)
	if gHideMode == 1 then
		wtBag:AddChild(wtHide)
		DnD.Widgets[ DnD.AllocateDnDID(wtHide) ].fLockedToParentArea = false
	end
	
	wtDays:SetVal("val", tostring(gDaysVariants[gDays]))
	wtDaysBag:SetVal("val", tostring(gDaysVariants[gDaysBag]))
	SaveConfig()
end

function SaveConfig()
	local saveConfig = {}
	
	saveConfig.gThings = wtCategories[gCategories[1]]:GetVariant()
	saveConfig.gCrafts = wtCategories[gCategories[2]]:GetVariant()
	saveConfig.gRarities = wtCategories[gCategories[3]]:GetVariant()
	
	gThings = wtCategories[gCategories[1]]:GetVariant()
	gCrafts = wtCategories[gCategories[2]]:GetVariant()
	gRarities = wtCategories[gCategories[3]]:GetVariant()
		
	saveConfig.gDays = gDays
	saveConfig.gDaysBag = gDaysBag
	saveConfig.gShow = gShow
	saveConfig.gHideMode = gHideMode
	
	userMods.SetGlobalConfigSection( "Settings", saveConfig )
end

function OnRefreshPress(params)
	if gMode == 1 then
		FillPanel(ITEM_CONT_DEPOSITE)
	else
		FillPanel(ITEM_CONT_INVENTORY)
	end
end

function Notification()
	local itemIds = avatar.GetInventoryItemIds()
	local notif = {}
	for _, v in pairs(itemIds) do
		if CheckItem(v) and math.floor(itemLib.GetTemporaryInfo( v ).remainingMs/ 60/ 60/ 24/ 1000) <= gDaysVariants[gDays] then
			table.insert(notif, {a = v})
		end
	end
	if table.maxn(notif) > 0 then
		table.sort(notif, function(a,b) return itemLib.GetTemporaryInfo(a.a).remainingMs < itemLib.GetTemporaryInfo(b.a).remainingMs end )
		Chat(TXT("NotifWarning"), "LogColorWhite", 18)
		for _, v in pairs(notif) do
			ChatItem(v.a, 16)
		end
	end
end

function OnBagShow(params)
	if params.widget:IsVisible() then
		common.RegisterEventHandler( function(params) StartTimer(gTimerInventoryChangedInventory, true) end, "EVENT_INVENTORY_SLOT_CHANGED" )
	
		if gShow then
			ShowPanel(true)
		end		
		if gDaysVariants[gDaysBag] == 0 then
			return
		end
		gPocket = -1
		RefreshItemTime()
		StartTimer(gTimer)
	else
		StopTimer(gTimer)
		ClearItemTime()
		if gShow then
			ShowPanel(false)
		end
	end
end

function OnTurnPress(params)
	gShow = not gShow
	wtHide:Show(not gShow)
	if gShow then
		wtMainPanel:Show(wtBag:IsVisible())
		if wtSettingsPanel:IsVisible() then
			wtSettingsPanel:Show(false)
		end
	end	
	wtButtonTurn:GetChildChecked("Lighting", true):Show(gShow)
	SaveConfig()
end

function OnDepositePress(params)
	local lighting = params.widget:GetChildChecked("Lighting", true)
	lighting:Show(not lighting:IsVisible())
	gMode = 1 - gMode
	ShowPanel(true)
end

function DnD.ITDropAttempt(params)
	Chat("Stoped")
	if params ~= wtHide then
		Chat("Not wtHide")
		return
	end
	if wtBag:IsVisible() then
		Chat("I see")
		local placementBag, placementHideBtn = wtBag:GetRealRect(), wtHide:GetRealRect()
		
		--Chat(placementBag.x1.." "..placementBag.x2.." "..placementBag.y1.." "..placementBag.y2)
		--Chat(placementHideBtn.posX.." "..placementHideBtn.posY)
		
		if gHideMode == 0 then
			if placementHideBtn.x1 >= placementBag.x1 and placementHideBtn.x2 <= placementBag.x2 and placementHideBtn.y1 >= placementBag.y1 and placementHideBtn.y2 <= placementBag.y2 then
				wtHide:SetVariant(1)
				gHideMode = 1
				wtBag:AddChild(wtHide)
				local pl = wtHide:GetPlacementPlain()
				pl.posX = 64
				pl.posY = 44
				pl.sizeX = 32
				pl.sizeY = 32
				wtHide:SetPlacementPlain(pl)	
				DnD.Widgets[ DnD.AllocateDnDID(wtHide) ].fLockedToParentArea = false
				SaveConfig()
				SetConfig( DnD.Widgets[ DnD.AllocateDnDID( wtHide ) ].CfgName, { posX = pl.posX, posY = pl.posY, highPosX = pl.highPosX, highPosY = pl.highPosY, sizeX = pl.sizeX, sizeY = pl.sizeY } )
			end
		else
			placementHideBtn = wtHide:GetPlacementPlain()
			if placementHideBtn.posX < 0 or placementHideBtn.posY < 0 or placementHideBtn.posX >= (placementBag.x2 - placementBag.x1) or placementHideBtn.posY >= (placementBag.y2 - placementBag.y1) then
				local screenParams = widgetsSystem:GetPosConverterParams()
				wtHide:SetVariant(0)
				gHideMode = 0
				wtBag:GetParent():AddChild(wtHide)
				local pl = wtHide:GetPlacementPlain()
				pl.posX = placementHideBtn.posX + placementBag.x1 * (screenParams.fullVirtualSizeX / screenParams.realSizeX)
				pl.posY = placementHideBtn.posY + placementBag.y1 * (screenParams.fullVirtualSizeY / screenParams.realSizeY)
				pl.sizeX = 30
				pl.sizeY = 30
				wtHide:SetPlacementPlain(pl)	
				DnD.Widgets[ DnD.AllocateDnDID(wtHide) ].fLockedToParentArea = true
				SaveConfig()
				SetConfig( DnD.Widgets[ DnD.AllocateDnDID( wtHide ) ].CfgName, { posX = pl.posX, posY = pl.posY, highPosX = pl.highPosX, highPosY = pl.highPosY, sizeX = pl.sizeX, sizeY = pl.sizeY } )
			end		
		end
	end
end

function CreateItems()
	local size = 140
	plItemNormal = wtItem:GetPlacementPlain()
	plItemNull = wtItem:GetPlacementPlain()
	plItemNull.sizeY = 0
	
	for i = 1,size do
		local _wtItem = mainForm:CreateWidgetByDesc(wtItem:GetWidgetDesc())	
		wtContainer:PushBack(_wtItem)
		table.insert(wtItems, _wtItem)	
		_wtItem:SetPlacementPlain(plItemNull)
	end
end

function UpdateBase(baseId)
	Chat("Updated "..baseId)
	if containerLib.IsOpen( baseId ) == false then return end
	local itemIds = containerLib.GetItems( baseId )
    gBase[baseId] = {}
    
	for i, v in pairs(itemIds) do
		if CheckItem(v, baseId) then
			table.insert(gBase[baseId], {pocket = avatar.InventoryGetItemPocket( v ), icon = itemLib.GetItemInfo( v ).icon, quality = itemLib.GetQuality(v).quality, name = itemLib.GetItemInfo(v).name, temporary = mission.GetGlobalDateTime().overallMs + itemLib.GetTemporaryInfo(v).remainingMs, count = itemLib.GetStackInfo( v ).count, id = v, index = i})
		end
    end
    
	table.sort(gBase[baseId], baseId == ITEM_CONT_INVENTORY and Comparison or function(a, b) return a.temporary < b.temporary end)
end

--------------------------------------------------------------------------------
-- INITIALIZATION
--------------------------------------------------------------------------------
function Init()	
	wtItem = mainForm:GetChildChecked("Item", true)
	wtSeparator = mainForm:GetChildChecked("Separator", true)
	wtContainer = mainForm:GetChildChecked("Container", true)
	
	wtItem:Show(false)
	
	wtMainPanel = cMainPanel()
	
	DnD.Init( wtHide, nil, true)
	
	--DnD.Resizer( mainForm:GetChildChecked("ResizeCorner", true), nil, true)
	--mainForm:GetChildChecked("ResizeCorner", true):SetBackgroundColor( { r = 1.0; g = 1.0; b = 1.0; a = 0.0 } )
	--mainForm:GetChildChecked("ResizeCorner", true):Show(true)

	common.RegisterReactionHandler( ShowHideBtnReaction, "ShowHideBtnReaction" )
	common.RegisterReactionHandler( ShowHideBtnReaction, "close_pressed" )
	common.RegisterReactionHandler( OnPressButtonDown, "slot_pressed" )
	common.RegisterReactionHandler( OnPointingButton, "slot_pointing" )
	common.RegisterReactionHandler( OnSettingsPress, "settings_pressed" )
	common.RegisterReactionHandler( OnCheckboxPress, "checkbox_pressed" )
	common.RegisterReactionHandler( OnRefreshPress, "refresh_pressed" )
	common.RegisterReactionHandler( OnUpPress, "up_pressed" )
	common.RegisterReactionHandler( OnSeparatorPointing, "separator_pointing" )
	common.RegisterReactionHandler( OnTurnPress, "turn_pressed" )
	common.RegisterReactionHandler( OnDepositePress, "deposite_pressed" )
	
	wtBag:SetOnShowNotification(true)
	--wtDepositeBox:SetOnShowNotification(true)
	
	common.RegisterEventHandler( OnBagShow, "EVENT_WIDGET_SHOW_CHANGED", {widget = wtBag})
	
	--common.RegisterEventHandler( OnDepositeBoxShow, "EVENT_WIDGET_SHOW_CHANGED", {widget = wtDepositeBox})
	
	ResetTextItem()
	
	CreateSettings()
	
	LoadConfig()
	
	InitTimer(Notification, 500, true)
	
	gTimer = InitTimer(RefreshItemTime, 250)
	gTimerInventoryChangedInventory = InitTimer(OnFormEffect, 250, false, ITEM_CONT_INVENTORY)
	gTimerInventoryChangedDeposite = InitTimer(OnFormEffect, 250, false, ITEM_CONT_DEPOSITE)
	
	UpdateBase(ITEM_CONT_DEPOSITE)
	UpdateBase(ITEM_CONT_INVENTORY)
	
	CreateItems()
	
	mainForm:SetPriority(10000)
end

--------------------------------------------------------------------------------
if avatar.IsExist() then
	Init()
else
	common.RegisterEventHandler( function() Init() common.UnRegisterEvent( "EVENT_AVATAR_CREATED" ) end, "EVENT_AVATAR_CREATED" )	
end
--------------------------------------------------------------------------------

