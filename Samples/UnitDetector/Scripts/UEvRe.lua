Global("EVRe", {})

--[[Global("qpq", nil)
Global("tttt", false)
Global("wtt",stateMainForm:GetChildUnchecked("Chat",true):GetChildUnchecked("Chat",true))
]]


function RegEv (ER)
for v,t in pairs(ER) do
	if string.find(v, "[A-Z]+T_+[A-Z]") then common.RegisterEventHandler(t,v) end
end
end

function RegERk (ER)
for v,t in pairs(ER) do
	if not string.find(v, "[A-Z]+T_+[A-Z]") then common.RegisterReactionHandler(t,v) end
end
end

function UnRegEv (ER)
for v,t in pairs(ER) do
	if string.find(v, "[A-Z]+T_+[A-Z]") then common.UnRegisterEventHandler(t,v) end
end
end

function UnRegERk (ER)
for v,t in pairs(ER) do
	if not string.find(v, "[A-Z]+T_+[A-Z]") then common.UnRegisterReactionHandler(t,v) end
end
end

function Reg (ER)
RegEv(ER)
RegERk(ER)
end

function UnReg (ER)
UnRegEv(ER)
UnRegERk(ER)
end

----- EVENTS -----
EVRe["SCRIPT_ADDON_INFO_REQUEST"] = function(params)
if params.target == common.GetAddonName() then
	userMods.SendEvent( "SCRIPT_ADDON_INFO_RESPONSE", {
		sender = params.target,
		desc = RD.ver[5].." v."..tostring(RD.ver[2]).." ["..RD.ver[3].."/"..RD.ver[4].."]"
		--showDNDButton = false,
		--showHideButton  = false,
		--showSettingsButton = false
		--addonsBlocked = {}
	})
end
end

EVRe["U_EVENT_ADDON_MEM_USAGE_REQUEST"] = function(params)
userMods.SendEvent( "U_EVENT_ADDON_MEM_USAGE_RESPONSE", {sender = common.GetAddonName(), memUsage = gcinfo()})
end

EVRe["EVENT_AVATAR_CREATED"] = function(params)
Sets.Lang = SetAddonLoc(Sets.Lang)
Sets.uDMetod = Sets.uDMetod=="norm" and avatar.GetUnitList and true or false

RD:Init()
wt:Init()
--common.LogInfo(common.GetAddonName(),"0 len ", tostring(table.getn(Lc)))
--common.LogInfo("", "Sets.uDMetod = ", tostring(Sets.uDMetod))
--common.LogInfo(common.GetAddonName(),"Init")
end

EVRe["EVENT_UNIT_LEVEL_CHANGED"] = function(params)
RD:DetectCh ()
end

EVRe["EVENT_UNIT_DEAD_CHANGED"] = function(params)
RD:DetectCh (params)
end

EVRe["EVENT_UNIT_PVP_FLAG_CHANGED"] = function(params)
RD:DetectCh (params)
end

EVRe["EVENT_UNITS_CHANGED"] = function(params)
	RD:UpUnitList(params.spawned);
	RD:DetectCh(params);
end
-- Ollaf 22.12.2014
EVRe["EVENT_AVATAR_CLIENT_ZONE_CHANGED"] = function(params)
	RD:UpUnitList(avatar.GetUnitList());
	RD:DetectCh({spawned = avatar.GetUnitList()});
end

EVRe["EVENT_DEVICES_CHANGED"] = function(params)
RD:ObjSearch (params.spawned)
--RD:DetectCh (params)
--[[for v,t in pairs(params.spawned) do
	if userMods.FromWString(object.GetName(t)) == "" then
	common.LogInfo("","obj not name")
	end
end]]


end

--[[EVRe["EVENT_AVATAR_PRIMARY_TARGET_CHANGED"] = function(params)
--EVRe["EVENT_AVATAR_SECONDARY_TARGET_CHANGED"] = function(params)
if wt.AT[1].idt then
	common.LogInfo("","DetachWidget2D ",tostring(wt.AT[1].idt))
	wt.AT[1].wdt:Show(false)
	--object.DetachWidget2D(wt.AT[1].idt)
	
	
	
else
	
	
	
	
	
end


wt.AT[1].idt = avatar.GetTarget()
--wt.AT[1].idt = avatar.GetSecondaryTarget()
--common.LogInfo("","-- ",tostring(wt.AT[1].idt))
if wt.AT[1].idt and object.IsDetected(wt.AT[1].idt) then
	--common.LogInfo("","IsDetected ",tostring(wt.AT[1].idt))
	--wt.AT[1].wdt:Show(false)
	object.AttachWidget2D( wt.AT[1].idt, wt.AT[1].wdt, ATTACHED_OBJECT_POS_CENTER )
	
	--wt.AT[1].wdt:Show(true)
	wt.AT[1].wdt:PlayResizeEffect( wt.AT[1].p1, wt.AT[1].p2, 5e3, EA_SYMMETRIC_FLASH )
	
	
else
	wt.AT[1].idt = nil
end
RD.targetid = wt.AT[1].idt

end]]


--[[EVRe["EVENT_EQUIPMENT_ITEM_CHANGED"] = function(params)
common.LogInfo("","GIIF ",tostring(params.itemId))
end]]

EVRe["EVENT_SECOND_TIMER"] = function(params)
Repaint()
--ReCountLenMark()
--[[if tttt then
	wtt:PopFront()
	tttt = false
end]]
end

--[[EVRe["EVENT_CAMERA_DIRECTION_CHANGED"] = function(params)
--RD:GetAngle (pos1,pos2,ang)
RD:UpdAngle()
end]]

--[[EVRe["EVENT_AVATAR_DIR_CHANGED"] = function(params)
--RD:GetAngle (pos1,pos2,ang)
RD:UpdAngle(params.dir)
end]]

--[[EVRe["EVENT_CANNOT_ATTACH_WIDGET_3D"] = function(params)
if params.objectId == wt.AT[1].id then
	common.LogInfo("","EVENT_CANNOT_ATTACH_WIDGET_3D ",tostring(params.objectId))
end


end]]



--[[
EVRe["EVENT_INSPECT_STARTED"] = function(params)
--common.LogInfo("","EVENT_INSPECT_STARTED")
local scn = RD:Scaning()
--common.LogInfo("","scn[1] ",tostring(scn[1]))
if scn[1]>0 then
	wt:ptsShow(scn,RD.stbl)
	avatar.EndInspect()
elseif scn[1]==0 then avatar.EndInspect()
elseif scn[1]<0 then
	RD.scanedid = avatar.GetSecondaryTarget()
	scn = RD:Scaning()
	wt.TextB:SetVal("text", common.FormatInt(scn[1],"%d"))
	--common.LogInfo("","!!scn[1] ",tostring(scn[1]))
end
end
--]]
--[[EVRe["EVENT_CHAT_MESSAGE"] = function(params)
--common.LogInfo("","EVENT_CHAT_MESSAGE")
--local wd = stateMainForm:GetChildUnchecked("Chat",true):GetChildUnchecked("ParentForm",true)
--local wd = stateMainForm:GetChildUnchecked("Chat",true):GetChildUnchecked("Chat",true)
--local valuedText = common.CreateValuedText()
if params.chatType == CHAT_TYPE_WORLD or params.chatType == CHAT_TYPE_YELL_ZONE then
--if params.chatType == CHAT_TYPE_WORLD or params.chatType == CHAT_TYPE_ZONE then
--common.LogInfo("","CHAT_TYPE_WORLD or CHAT_TYPE_ZONEYELL")
	--wd:PopFront()
	wtt:PushBackRawText( params.sender )
	--wd:Enable(true)
	tttt = true
	
	
end

end]]

---- Reactions ----
EVRe["ButtonC_clk"] = function(reaction)
--local n = unit.GetMoodEmoteId and unit.GetMoodEmoteId(avatar.GetId())
--if n then common.LogInfo(common.GetAddonName(),avatar.GetEmoteInfo(n).sysName) end
--wt:goCSW(true)

if not Sets.uDMetod then return end
local nid = tonumber(string.sub(reaction.widget:GetName(),3))
if nid and object.IsExist( nid ) then avatar.SelectTarget(nid) end
end

EVRe["ShowHideBtnReaction"] = function(reaction)
	if Dnd.IsDragging() then
		return
	end
	button_state = 1 - button_state
	if button_state == 0 then
		mainForm:GetChildChecked("ShowHideBtn", false):SetTextColor(nil, button_off )
	else
		mainForm:GetChildChecked("ShowHideBtn", false):SetTextColor(nil, button_on )
	end
end

EVRe["ButtonC_rclk"] = function(reaction)
local id, itms

--userMods.SendEvent( "EVENT_MAILBOX_CHANGED", {} )


if string.find(reaction.widget:GetName(),"S_") then
	if wt.LineEdit:IsVisible()then
		wt.LineEdit:Show(false)
	else
		wt.LineEdit:SetText(RD.searched)
		wt.LineEdit:Show(true)
		wt.LineEdit:SetFocus(true)
	end
elseif string.find(reaction.widget:GetName(),"U_") then
	--RD:StartScan(tonumber(string.sub(reaction.widget:GetName(),3)))
	if object.IsExist( tonumber(string.sub(reaction.widget:GetName(),3)) ) then
		if avatar.IsInspectAllowed() and unit.IsPlayer( tonumber(string.sub(reaction.widget:GetName(),3)) ) then
			avatar.StartInspect(tonumber(string.sub(reaction.widget:GetName(),3)))
		end
	end
	--qpq = tonumber(string.sub(reaction.widget:GetName(),3))
	--GS.RequestInfo(qpq)
	--avatar.SelectTarget(qpq)
	--avatar.StartInspect()
end
end

EVRe["ButtonC_pnt"] = function(reaction)
if reaction.active and PrepareTT(reaction.widget) then
	wt:myTooltip(reaction.widget)
	--RD:StartScan(tonumber(string.sub(reaction.widget:GetName(),3)))
else
	wt:myTooltip()
	wt.PanelPt:Show(false)
end
end

EVRe["ButtonM_clk"] = function(reaction)
--RD:Selfscan()
if Dnd.IsDragging() then return end
wt:EnableUP(not wt.PanelUnit:IsVisible())

--common.LogInfo( "Blah", tostring(rawequal(userMods.ToWString("wee"),userMods.ToWString("wee"))))
--common.StateUnloadManagedAddon( "UserAddon/"..common.GetAddonName() )
--local trans = unit.GetTransport(avatar.GetId())
   --local devs = transport.GetDevices(unit.GetTransport(avatar.GetId()))
   --[[for i, v in pairs(transport.GetDevices(unit.GetTransport(avatar.GetId()))) do
      common.LogInfo( "Blah", "Name = "..userMods.FromWString(object.GetName(v)))
   end]]

--[[local em
for v,t in pairs(avatar.GetEmotes()) do
	em = avatar.GetEmoteInfo(t)
	if em then
		common.LogInfo(common.GetAddonName(),"name ",em.name)
		common.LogInfo(common.GetAddonName(),"...hasMood ",tostring(em.hasMood))
		common.LogInfo(common.GetAddonName(),"...description ", em.description)
		common.LogInfo(common.GetAddonName(),"...sysName ", em.sysName)
		if em.sysName == "lonelyMood" then avatar.RunEmote(t) end
	end
end]]

end

EVRe["ButtonM_rclk"] = function(reaction)
--wt:EnableUP(not wt.PanelUnit:IsVisible())
if DnD.wtMov[1][4] then DnDUnReg(1) else DnDReg(1) end
end

EVRe["rEn"] = function(reaction)
local sch = reaction.widget:GetText()
if (string.len(userMods.FromWString(sch))>2) or (string.len(userMods.FromWString(sch))<1) then
	RD.searched = sch
	--common.LogInfo(common.GetAddonName(),"reaction ",sch)
	--[[wt.LineEdit:Show(false)
	wt.AT[1].wdt:Show(false)
	wt.AT[1].id = nil
	wt:reTarget()]]
	--wt.AT[1].wdt:Show(false)
	RD:Init()
	wt:nS(string.len(userMods.FromWString(sch))>0)
end
end

EVRe["rESC"] = function(reaction)
wt.LineEdit:Show(false)
end

EVRe["PPt_point"] = function(reaction)
--[[common.LogInfo("","PPt_point")
for v,p in pairs(reaction) do
	common.LogInfo("",tostring(v)," - ",tostring(p))
end]]

--if reaction.active then
	--reaction.widget:Show(false)
--end

--wt.PanelPt:Show(false)
end