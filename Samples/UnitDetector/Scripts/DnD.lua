--DnD ver. 01-14/09/2011 <Nikon>
--DnD functions
Global("DnDEV", {})
Global("DnD", {})
local DnDStep = 10

function DnDInit()
for k, v in pairs(DnDEV) do
	common.RegisterEventHandler(v, k)
end
DnD.ID = DND_WIDGET_MOVE * DND_CONTAINER_STEP + 579
DnD.wtMov = {}
--Main Panel
DnD.wtMov[1] = {
	mainForm:GetChildUnchecked("ButtonM", false),--че таскаем
	wt.ButtonInfo,		--за че таскаем
	RD.ver[1].."_"..(DnD.ID+1),		--ИД
	false		--начальная активность ДНД
	}
--Panel Info
--[[DnD.wtMov[2] = {
	qe.PanelBS,
	qe.Container:GetChildUnchecked("ButtonCHeader", false),
	qe.ver[1].."_"..(DnD.ID+2),
	true
	}]]
--Panel Graph
--[[DnD.wtMov[3] = {
	mainForm:GetChildUnchecked("PanelGraph", false),
	mainForm:GetChildUnchecked("PanelGraph", false),
	common.GetAddonName().."_"..(DnD.ID+3).."_"..srevision
	}
--Panel Graph
DnD.wtMov[4] = {
	mainForm:GetChildUnchecked("PanelGraph", false),
	PTick[MaxTicks+1],
	common.GetAddonName().."_"..(DnD.ID+3).."_"..srevision
	}
--Sleep button
DnD.wtMov[5] = {
	Slp,
	Slp,
	common.GetAddonName().."_"..(DnD.ID+4).."_"..srevision
	}]]
DnD.IsPicked = false
DnD.wtMovable = DnD.wtMov[1][1]
DnD.Screen = widgetsSystem:GetPosConverterParams()
for i=1,1 do DnDReg(i,not DnD.wtMov[i][4]) end
end

function DnDReg (num,flg)
local pos
DnD.Place = {}
DnD.SCSection = DnD.wtMov[num][3]
DnD.Place = DnD.wtMov[num][1]:GetPlacementPlain()
pos = userMods.GetGlobalConfigSection(DnD.SCSection)
DnD.Place.posX = pos and pos.posX or 50
DnD.Place.posY = pos and pos.posY or 300
DnD.Place.posX = (DnD.Screen.fullVirtualSizeX > (DnD.Place.posX + 0.5 * DnD.Place.sizeX)) and DnD.Place.posX or DnD.Screen.fullVirtualSizeX - 0.5 * DnD.Place.sizeX
DnD.Place.posY = (DnD.Screen.fullVirtualSizeY > (DnD.Place.posY + 50)) and DnD.Place.posY or DnD.Screen.fullVirtualSizeY - 50
DnD.Place.posX = ((DnD.Place.posX - DnD.Place.sizeX) > 1.5 * -DnD.Place.sizeX) and DnD.Place.posX or 0
DnD.Place.posY = (DnD.Place.posY > 0) and DnD.Place.posY or 0

DnD.wtMov[num][1]:SetPlacementPlain(DnD.Place)
if not flg then
	mission.DNDRegister(DnD.wtMov[num][2], DnD.ID+num, true )
	DnD.wtMov[num][4] = true
end
end

function DnDUnReg (num)
DnD.wtMov[num][4] = false
mission.DNDUnregister(DnD.wtMov[num][2])
end

---
DnDEV["EVENT_DND_PICK_ATTEMPT"] = function(params)
--common.LogInfo(common.GetAddonName(),"EVENT_DND_PICK_ATTEMPT")
if (params.srcId > DnD.ID) and (params.srcId <= DnD.ID+table.getn(DnD.wtMov)) then
	DnD.wtMovable =  DnD.wtMov[params.srcId-DnD.ID][1]
	DnD.SCSection = DnD.wtMov[params.srcId-DnD.ID][3]
	--if CheckVer() > 2000 then mission.DNDConfirmPickAttempt() end
	if group.IsLeader then mission.DNDConfirmPickAttempt() end	--AO 2.0+
	DnD.Screen = widgetsSystem:GetPosConverterParams()
	DnD.Place = DnD.wtMovable:GetPlacementPlain()
	DnD.DeltaX = params.posX * DnD.Screen.fullVirtualSizeX / DnD.Screen.realSizeX - DnD.Place.posX
	DnD.DeltaY = params.posY * DnD.Screen.fullVirtualSizeY / DnD.Screen.realSizeY - DnD.Place.posY
	--common.SetCursor( "drag" )
	DnD.IsPicked = true
end
end

DnDEV["EVENT_DND_DROP_ATTEMPT"] = function(params)
--common.LogInfo(common.GetAddonName(),"EVENT_DND_DROP_ATTEMPT")
if DnD.IsPicked then
	local XY = {posX = DnD.wtMovable:GetPlacementPlain().posX,
				posY = DnD.wtMovable:GetPlacementPlain().posY}
	mission.DNDConfirmDropAttempt()
	--common.SetCursor( "default" )
	DnD.IsPicked = false
	userMods.SetGlobalConfigSection(DnD.SCSection, XY)
end
end

DnDEV["EVENT_DND_DRAG_TO"] = function(params)
--common.LogInfo(common.GetAddonName(),"EVENT_DND_DRAG_TO")
if DnD.IsPicked then
	DnD.Place.posX = params.posX * DnD.Screen.fullVirtualSizeX / DnD.Screen.realSizeX - DnD.DeltaX
	DnD.Place.posY = params.posY * DnD.Screen.fullVirtualSizeY / DnD.Screen.realSizeY - DnD.DeltaY
	DnD.Place.posX = DnDStep*math.floor(DnD.Place.posX/DnDStep)
	DnD.Place.posY = DnDStep*math.floor(DnD.Place.posY/DnDStep)
	DnD.wtMovable:SetPlacementPlain( DnD.Place )
	--common.SetCursor( "drag" )
end
end