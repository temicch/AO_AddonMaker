Global("Stats", {
"Удача",
"Ярость",
"Стремительность",
"Господство",
"Упорство",

"Живучесть",
"Осторожность",
"Незыблемость",
"Инстинкт",
"Скорость",
})

Global( "equipmentSlots", 
{
	["EquipmentSlot01"] = DRESS_SLOT_HELM,
	["EquipmentSlot02"] = DRESS_SLOT_MANTLE,
	["EquipmentSlot03"] = DRESS_SLOT_CLOAK,
	["EquipmentSlot04"] = DRESS_SLOT_ARMOR,
	["EquipmentSlot05"] = DRESS_SLOT_GLOVES,
	["EquipmentSlot06"] = DRESS_SLOT_BELT,
	["EquipmentSlot07"] = DRESS_SLOT_PANTS,
	["EquipmentSlot08"] = DRESS_SLOT_BOOTS,
		
	["EquipmentSlot09"] = DRESS_SLOT_EARRING2,
	["EquipmentSlot10"] = DRESS_SLOT_EARRING1,
	["EquipmentSlot11"] = DRESS_SLOT_NECKLACE,
	["EquipmentSlot13"] = DRESS_SLOT_SHIRT,
	["EquipmentSlot14"] = DRESS_SLOT_BRACERS,
	["EquipmentSlot15"] = DRESS_SLOT_RING1,
	["EquipmentSlot16"] = DRESS_SLOT_RING2,
	
	["EquipmentSlot17"] = DRESS_SLOT_MAINHAND,
	["EquipmentSlot18"] = DRESS_SLOT_RANGED,
	["EquipmentSlot19"] = DRESS_SLOT_OFFHAND
})

local specWidgets = 
{
	["Мастерство"] = {},
	["Решимость"] = {},
	["Беспощадность"] = {},
	["Стойкость"] = {},
	["Кровожадность"] = {},
}

local allStats = {}

for i, v in pairs(specWidgets) do
	table.insert(allStats, i)
end
for i, v in pairs(Stats) do
	table.insert(allStats, v)
end

for _, v in pairs(Stats) do
	specWidgets[v] = {}
end

Global("SpecialStatsWidgets", {})
Global("specIds", {})

Global("wtMainPanel", 			mainForm:	GetChildChecked("MainPanel", false))
Global("wtSpecialStatsPanel", 	wtMainPanel:GetChildChecked("SpecialStatsPanel", true))
Global("wtSpecialStatsPanelInn", 	wtMainPanel:GetChildChecked("SpecialStatsPanelInn", true))

Global("wtUp", mainForm:GetChildChecked("MainPanel", false):GetChildChecked("ButtonUp", false))
Global("wtDown", mainForm:GetChildChecked("MainPanel", false):GetChildChecked("ButtonDown", false))

Global("wtMast", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtMast)
Global("wtResh", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtResh)
Global("wtBesp", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtBesp)
Global("wtStoik", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtStoik)
Global("wtKrov", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtKrov)

Global("wtPower", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtPower)
Global("wtStamina", mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text1", true):GetWidgetDesc()))
wtMainPanel:AddChild(wtStamina)

wtMast:SetName("Мастерство")
wtBesp:SetName("Беспощадность")
wtResh:SetName("Решимость")
wtStoik:SetName("Стойкость")
wtKrov:SetName("Кровожадность")

local place = wtMast:GetPlacementPlain()
place.posX = 52
place.posY = 134
wtMast:SetPlacementPlain( place )
place.posX = 52
place.posY = 198
wtResh:SetPlacementPlain( place )
place.posX = 52
place.posY = 264
wtBesp:SetPlacementPlain( place )
place.posX = 175
place.posY = 134
wtStoik:SetPlacementPlain( place )
place.posX = 175
place.posY = 264
wtKrov:SetPlacementPlain( place )

place.posX = 52-10
place.posY = 80
wtPower:SetPlacementPlain( place )
place.posX = 175-10
place.posY = 80
wtStamina:SetPlacementPlain( place )

wtMast:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )
wtResh:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )
wtBesp:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )
wtStoik:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )
wtKrov:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )

wtPower:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )
wtStamina:SetTextColor(nil, { r = 228/255; g = 207/255; b = 159/255; a = 0.9 } )

function OnInspectStart( params )
	if not params.widget:IsVisibleEx() or not avatar.IsInspectAllowed() then
		wtMainPanel:Show(false)
		return
	end
	local targetId = avatar.GetInspectInfo().playerId
	if not targetId or not avatar.IsTargetInspected() then
		return
	end
	for i,_ in pairs(specIds) do
		specIds[i] = nil
	end
	wtMainPanel:Show(true)	
	local pl = wtSpecialStatsPanelInn:GetPlacementPlain()
	pl.posY = 0
	wtSpecialStatsPanelInn:SetPlacementPlain(pl)
	for i, _ in pairs(specWidgets) do
		specWidgets[i] = {}
	end
	for z, i in pairs(equipmentSlots) do
		local bonus = unit.GetEquipmentSlotBonus( targetId, i, ITEM_CONT_EQUIPMENT )
		if bonus then
		
			specIds["Могущество"] = (specIds["Могущество"] or 0) + bonus.miscStats.power.effective
			specIds["Выносливость"] = (specIds["Выносливость"] or 0) + bonus.miscStats.stamina.effective
		
			for _,v in pairs(bonus.specStats) do
				specIds[userMods.FromWString(v.tooltipName)] = (specIds[userMods.FromWString(v.tooltipName)] or 0) + v.value
				if specWidgets[userMods.FromWString(v.tooltipName)] == nil then
					specWidgets[userMods.FromWString(v.tooltipName)] = {}
					LogInfo(userMods.FromWString(v.tooltipName))
				end
				table.insert(specWidgets[userMods.FromWString(v.tooltipName)], z)
			end
		end
		--[[
		bonus = unit.GetEquipmentSlotBonus( targetId, i, ITEM_CONT_EQUIPMENT_RITUAL )
		if bonus then
		
			specIds["Могущество_RITUAL"] = (specIds["Могущество_RITUAL"] or 0) + bonus.miscStats.power.effective
			specIds["Выносливость_RITUAL"] = (specIds["Выносливость_RITUAL"] or 0) + bonus.miscStats.stamina.effective
		
			for _,v in pairs(bonus.specStats) do
				specIds[userMods.FromWString(v.tooltipName)] = (specIds[userMods.FromWString(v.tooltipName)] or 0) + v.value
				table.insert(specWidgets[userMods.FromWString(v.tooltipName)], z)
			end
		end
		]]--
	end
	wtPower:	SetVal("val", common.FormatFloat(specIds["Могущество"] or 0, "%.2f"))
	wtStamina:	SetVal("val", common.FormatFloat(specIds["Выносливость"] or 0, "%.2f"))
	
	wtMast:	SetVal("val", common.FormatFloat(specIds["Мастерство"] or 0, "%.2f"))
	wtResh:	SetVal("val", common.FormatFloat(specIds["Решимость"] or 0, "%.2f"))
	wtBesp:	SetVal("val", common.FormatFloat(specIds["Беспощадность"] or 0, "%.2f"))
	wtStoik:SetVal("val", common.FormatFloat(specIds["Стойкость"] or 0, "%.2f"))
	wtKrov:	SetVal("val", common.FormatFloat(specIds["Кровожадность"] or 0, "%.2f"))
	for i, statName in pairs(Stats) do
		SpecialStatsWidgets[i]:SetVal("val", common.FormatFloat(specIds[statName] or 0, "%.2f"))	
	end
end

local wtInspectMainForm = common.GetAddonMainForm( "InspectCharacter" )
		
function OnPoint(params)
	if params.active == true then
		if GetTableSize(specWidgets[params.sender]) == 0 then
			return
		end
		for _,v in pairs(specWidgets[params.sender]) do
			local wtEq = wtInspectMainForm:GetChildChecked(v, true)
			wtEq:GetChildChecked("Autocast", false):Show(true)
			wtEq:GetChildChecked("Autocast", false):PlayFadeEffect( 0.7, 1.0, 800, EA_SYMMETRIC_FLASH )
		end	
	else
		for i, _ in pairs(equipmentSlots) do
			local wtEq = wtInspectMainForm:GetChildChecked(i, true)
			wtEq:GetChildChecked("Autocast", false):Show(false)
		end
	end
end

function OnPressButtonUp()
	if wtDown:IsVisible() then
		return
	end
	local pl = wtSpecialStatsPanelInn:GetPlacementPlain()
	pl.posY = pl.posY + 231
	wtSpecialStatsPanelInn:SetPlacementPlain(pl)
	wtDown:Show(true)
	wtUp:Show(false)
	
	for i, statName in pairs(Stats) do
		SpecialStatsWidgets[i]:SetVal("val", common.FormatFloat(specIds[statName] or 0, "%.2f"))
		SpecialStatsWidgets[i]:SetName(statName)
		if i > 8 then	
			break
		end
	end		
end

function OnPressButtonDown()
	if wtUp:IsVisible() then
		return
	end
	local pl = wtSpecialStatsPanelInn:GetPlacementPlain()
	pl.posY = pl.posY - 231
	wtSpecialStatsPanelInn:SetPlacementPlain(pl)
	wtDown:Show(false)
	wtUp:Show(true)
	for i, statName in pairs(Stats) do
		if i > 9 then
			SpecialStatsWidgets[i - 9]:SetVal("val", common.FormatFloat(specIds[statName] or 0, "%.2f"))
			SpecialStatsWidgets[i - 9]:SetName(statName)
		end
	end	
end

local buffsPattern = {"Показатель", "показатель"}

function OnSaveButtonPress(params)
	if not avatar.IsInspectAllowed() then return end
	if avatar.GetInspectInfo() == nil then return end
	if avatar.GetInspectInfo().playerId == nil then return end
	local unitId = avatar.GetInspectInfo().playerId
	
	--local unitId = avatar.GetId()
	
	local guildInfo = unit.GetGuildInfo( unitId )
	LogInfo("-----------------------------------------------")
	LogInfo(string.format("%-21s","Player:")..userMods.FromWString(object.GetName(unitId)))
	if guildInfo then
		LogInfo(string.format("%-21s","Guild")..userMods.FromWString(guildInfo.name).." ("..guildInfo.level..")")	
	end
	LogInfo(string.format("%-21s","GS:")..math.floor(unit.GetGearScore( unitId )))
	LogInfo(string.format("%-21s","Class:")..unit.GetClass( unitId ).className)
	LogInfo("")	
	for _, v in pairs(object.GetBuffs( unitId )) do
		local buffInfo = object.GetBuffInfo( v )
		local buffName = userMods.FromWString(buffInfo.name)
		if buffName then
			if string.find(buffName, "Аспект") ~= nil then
				LogInfo(buffName)					
			end			
		end
		if buffInfo.description then  
			local buffDesc = userMods.FromWString(common.ExtractWStringFromValuedText(buffInfo.description))
			for _, z in pairs(buffsPattern) do
				if string.find(buffDesc, z) ~= nil then
					LogInfo(buffName..": "..string.format("%-21s",buffDesc))				
				end
			end
		end
	end	
	LogInfo("")	
	for i, statName in pairs(allStats) do
		LogInfo(string.format("%-21s",statName..":")..userMods.FromWString(common.FormatFloat(specIds[statName] or 0, "%.1f")))
	end	
	LogInfo("-----------------------------------------------")
end

function InitStart(params)
	if not avatar.IsExist() then
		return
	end
	
	--local guildInfo = unit.GetGuildInfo( avatar.GetId() )
	--local name = guildInfo and guildInfo.name
	--if not common.IsSubstring(object.GetName(avatar.GetId()), userMods.ToWString("Стайкрай")) or not common.IsSubstring(name, userMods.ToWString("Ударная Волна"))  then
	--if not common.IsSubstring(name or nil, userMods.ToWString("Ударная Волна"))  then
	--	LogToChat("Этот аддон не предназначен для вас")
	--	return
	--end
	
	common.RegisterEventHandler( OnInspectStart, "EVENT_WIDGET_SHOW_CHANGED" )
	
	--common.RegisterReactionHandler( OnPressButtonUp, "OnPressButtonUp" )
	--common.RegisterReactionHandler( OnPressButtonDown, "OnPressButtonDown" )
	common.RegisterReactionHandler( OnPoint, "OnPoint" )
	common.RegisterReactionHandler( OnSaveButtonPress, "OnPressButtonSave" )
	
	local wtInspectMainForm = common.GetAddonMainForm( "InspectCharacter" )
	local pl1 = wtInspectMainForm:GetPlacementPlain()
	local pl2 = wtMainPanel:GetPlacementPlain()
	local pl3 = wtInspectMainForm:GetChildChecked("InfoPanel", true):GetPlacementPlain()
	pl2.posX = pl1.posX + pl1.sizeX/2 + 20
	pl2.posY = pl1.posY + pl3.posY + pl3.sizeY - 13
	wtMainPanel:SetPlacementPlain(pl2)
	
	wtInspectMainForm:GetChildChecked("InfoPanel", true):SetOnShowNotification( true )
	
	local y = 8
	for i, statName in pairs(Stats) do
		SpecialStatsWidgets[i] = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())
		
		local place = SpecialStatsWidgets[i]:GetPlacementPlain()
		place.posX = 164
		place.posY = y
		SpecialStatsWidgets[i]:SetPlacementPlain( place )
		
		SpecialStatsWidgets[i]:SetName(statName)
		SpecialStatsWidgets[i]:SetTextColor(nil, { r = 255/255; g = 231/255; b = 178/255; a = 0.9 } )

		local label = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())
		
		local place = label:GetPlacementPlain()
		place.posX = 15
		place.posY = y
		place.sizeX = 150
		label:SetPlacementPlain( place )
		
		label:SetName(statName)
		label:SetFormat("<header fontsize=\"12\"><rs class=\"class\"><html outline=\"0\" shadow=\"0\" outlinecolor=\"0x000000\"><r name=\"val\"/></html></rs></header>")
		label:SetTextColor(nil, { r = 255/255; g = 231/255; b = 178/255; a = 0.9 } )
		label:SetVal("val", statName)		
				
		y = y + 24
		
		wtSpecialStatsPanel:AddChild(SpecialStatsWidgets[i])
		wtSpecialStatsPanel:AddChild(label)
	end
end

function Init()
	common.RegisterEventHandler( InitStart, "EVENT_AVATAR_CREATED" )
end
--------------------------------------------------------------------------------
if avatar.IsExist() then InitStart() end
Init()
--------------------------------------------------------------------------------