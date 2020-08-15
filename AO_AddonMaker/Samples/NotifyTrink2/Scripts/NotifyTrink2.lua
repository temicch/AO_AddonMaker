Global("LightText", mainForm:GetChildChecked("LightText", false))
Global("DarkText", mainForm:GetChildChecked("DarkText", false))

Global("LightWing", mainForm:GetChildChecked("LightWing", false))
Global("DarkWing", mainForm:GetChildChecked("DarkWing", false))

Global("LightRemaining", mainForm:GetChildChecked("LightRemaining", false))
Global("DarkRemaining", mainForm:GetChildChecked("DarkRemaining", false))

Global("BuffLight", nil)
Global("BuffDark", nil)


local LightName = GTL( "LightName" )
local DarkName = GTL( "DarkName" )

--- Названия бафов от амулетов с астрала ----
-- Можно подставить в LightName и DarkName --
-- Сила  - Чары Несокрушимого Бойца
-- Разум - Чары Гениального Озарения
-- Удача - Чары Любимца Судьбы
-- Вынос - Чары Вечной Жизни
---------------------------------------------

local ready = userMods.ToWString("++")

local LightTime = 0
local DarkTime  = 0

local TimeTrackerLight = nil
local TimeTrackerDark  = nil

local wing_offset      = 400   -- Расстояние между каждым крылом и краем экрана
local max_text         = 1.0   -- Масштаб крупной цифры
local min_text         = 0.8   -- Масштаб уменьшенной цифры
local time_text_scale  = 1000  --
local wing_fade        = 0.7   -- Прозрачность крыльев

local config = userMods.GetGlobalConfigSection("NotifyTrink2")
if config and config["wing_offset"] and type(config["wing_offset"]) == "number" then
	wing_offset = config["wing_offset"]
end

-------------------------------------------------------------------------------
-- FUNCTION
-------------------------------------------------------------------------------

rawset( math, "round", function( number, limit )
	local multiplier = 10 ^ ( limit or 0 )
	return math.floor( number * multiplier + 0.5 ) / multiplier
end )

function DeadChanged(p)
	if p.unitId == avatar.GetId() then
		TimeTrackerLight = nil
		TimeTrackerDark = nil
		BuffLight = nil
		BuffDark = nil
		LightText:Show(false)
		LightWing:Show(false)
		DarkText:Show(false)
		DarkWing:Show(false)
	end
end

----------------------------------------------
----------------------------------------------
----------------------------------------------

function Buffs(p)
	local buffs
	if p.objectId then
		buffs = object.GetBuffInfo(p.buffId)
	else
		buffs = unit.GetBuff(p.unitId, p.index)
		p.buffId = buffs.buffId
	end
	if p.objectId then
		p.unitId = p.objectId
	end
	if p.unitId and p.unitId == avatar.GetId() then
		if buffs.isNeedVisualize then
			if buffs.texture then
				
				if userMods.FromWString(buffs.name) == LightName then					
					BuffLight = p.buffId
					if buffs.durationMs > 0 and buffs.remainingMs > 0 then
						local times = buffs.remainingMs							
						times = math.round( times / 1000 )
						TimeTrackerLight = true
						LightText:SetVal("Time", common.FormatInt( times, "%d" ))
						LightText:PlayTextScaleEffect( max_text, min_text, time_text_scale, EA_MONOTONOUS_INCREASE )
						LightWing:Show(true)
						LightText:Show(true)
						LightTime = 60
					end
				end
				
				if userMods.FromWString(buffs.name) == DarkName then
					BuffDark = p.buffId
					if buffs.durationMs > 0 and buffs.remainingMs > 0 then
						local times = buffs.remainingMs						
						times = math.round( times / 1000 )
						TimeTrackerDark = true
						DarkText:SetVal("Time", common.FormatInt( times, "%d" ))
						DarkText:PlayTextScaleEffect( max_text, min_text, time_text_scale, EA_MONOTONOUS_INCREASE )
						DarkWing:Show(true)
						DarkText:Show(true)
						DarkTime = 60
					end						
				end
				
			end
		end
	end
end

function ExistLightCheck(p)
	local notExist = true
	for i, v in pairs(object.GetBuffs(p.unitId)) do
		if v == BuffLight then
			notExist = false
			break
		end
	end
	if notExist == true then
		TimeTrackerLight = nil
		BuffLight = nil	
		LightText:Show(false)
		LightWing:Show(false)
	end
end

function ExistDarkCheck(p)
	local notExist = true
	for i, v in pairs(object.GetBuffs(p.unitId)) do
		if v == BuffDark then
			notExist = false
			break
		end
	end
	if notExist == true then
		TimeTrackerDark = nil
		BuffDark = nil
		DarkText:Show(false)
		DarkWing:Show(false)
	end
end

function BuffsChange(p)
	if p.objectId then
		p.unitId = p.objectId
	end
	if object.IsUnit(p.unitId) and p.unitId == avatar.GetId() then
		if BuffLight then
			ExistLightCheck(p)
		end
		if BuffDark then
			ExistDarkCheck(p)
		end
	end
end
	
function SecondTimer()
	if TimeTrackerLight then
		if BuffLight then
			local buffs
			for i, v in pairs(object.GetBuffs(avatar.GetId())) do
				if v == BuffLight then
					buffs = object.GetBuffInfo(v)
					end
			end
			if buffs then
				local times = buffs.remainingMs				
				if times <= 2000 then
					local tf = LightText:GetFade()
					local wf = LightWing:GetFade()
					LightText:PlayFadeEffect( tf, 0.0, math.max(times, 1), EA_MONOTONOUS_INCREASE )
					LightWing:PlayFadeEffect( wf, 0.0, math.max(times, 1), EA_MONOTONOUS_INCREASE )
					else
						LightText:SetFade( 1.0 )
						LightWing:SetFade( wing_fade )
				end				
				times = math.round( times / 1000 )
				TimeTrackerLight = true
				LightText:SetVal("Time", common.FormatInt( times, "%d" ))
				LightText:PlayTextScaleEffect( max_text, min_text, time_text_scale, EA_MONOTONOUS_INCREASE )
			end
		end
	end
	
	if TimeTrackerDark then
		if BuffDark then
			local buffs
			for i, v in pairs(object.GetBuffs(avatar.GetId())) do
				if v == BuffDark then
					buffs = object.GetBuffInfo(v)
					end
			end
			if buffs then
				local times = buffs.remainingMs					
				if times <= 2000 then
					local tf = DarkText:GetFade()
					local wf = DarkWing:GetFade()
					DarkText:PlayFadeEffect( tf, 0.0, math.max(times, 1), EA_MONOTONOUS_INCREASE )
					DarkWing:PlayFadeEffect( wf, 0.0, math.max(times, 1), EA_MONOTONOUS_INCREASE )
					else
						DarkText:SetFade( 1.0 )
						DarkWing:SetFade( wing_fade )
				end	
				times = math.round( times / 1000 )
				TimeTrackerDark = true
				DarkText:SetVal("Time", common.FormatInt( times, "%d" ))
				DarkText:PlayTextScaleEffect( max_text, min_text, time_text_scale, EA_MONOTONOUS_INCREASE )
			end
		end
	end
	
	if LightTime > 0 then
		LightRemaining:SetVal("value", common.FormatInt( LightTime, "%02d"))
		LightTime = LightTime - 1
	else
		LightRemaining:SetVal("value", ready)
	end
	if DarkTime > 0 then
		DarkRemaining:SetVal("value", common.FormatInt( DarkTime, "%02d"))
		DarkTime = DarkTime - 1
	else
		DarkRemaining:SetVal("value", ready)
	end
	
end

function Slash(p)
	local t = userMods.FromWString(p.text)
	if string.sub(t, 1, 3) == "/nt" then
		local offset = tonumber(string.sub(t, 5))
		if type(offset) == "number" then 
			wing_offset = offset 
			wingPlacement()
			userMods.SetGlobalConfigSection("NotifyTrink2",{["wing_offset"] = wing_offset})
		end
	end
end

function wingPlacement()
	local place
	
	place = DarkWing:GetPlacementPlain()
	place.posX = wing_offset
	DarkWing:SetPlacementPlain(place)
	
	place = DarkText:GetPlacementPlain()
	place.posX = wing_offset + 85
	DarkText:SetPlacementPlain(place)
	----
	place = LightWing:GetPlacementPlain()
	place.highPosX = wing_offset
	LightWing:SetPlacementPlain(place)
	
	place = LightText:GetPlacementPlain()
	place.highPosX = wing_offset + 85
	LightText:SetPlacementPlain(place)
	----
	place = DarkRemaining:GetPlacementPlain()
	place.posX = 405
	place.highPosY = 85
	DarkRemaining:SetPlacementPlain(place)
	DarkRemaining:SetVal("value", ready)
	
	place = LightRemaining:GetPlacementPlain()
	place.posX = 405
	place.highPosY = 65
	LightRemaining:SetPlacementPlain(place)	
	LightRemaining:SetVal("value", ready)
	----

end
--------------------------------------------------------------------------------
-- INITIALIZATION
--------------------------------------------------------------------------------
function Init()
	
	wingPlacement()
	LightWing:SetFade( wing_fade )
	LightWing:Show(false)
	DarkWing:SetFade( wing_fade )
	DarkWing:Show(false)
	
	common.RegisterEventHandler(DeadChanged, "EVENT_UNIT_DESPAWNED")
	common.RegisterEventHandler(DeadChanged, "EVENT_UNIT_DEAD_CHANGED")
	common.RegisterEventHandler( Buffs, "EVENT_OBJECT_BUFF_ADDED")
	common.RegisterEventHandler( BuffsChange, "EVENT_OBJECT_BUFFS_CHANGED" )
	common.RegisterEventHandler( SecondTimer, "EVENT_SECOND_TIMER" )
	common.RegisterEventHandler( Slash, "EVENT_UNKNOWN_SLASH_COMMAND" )
	
end

--------------------------------------------------------------------------------
common.RegisterEventHandler(Init, "EVENT_AVATAR_CREATED")
if avatar.IsExist() then
	Init()
end
--------------------------------------------------------------------------------