if not unit.GetHealthPercentage then unit.GetHealthPercentage = function (id) return object.GetHealthInfo(id).valuePercents end end
--[[
Global ("IS", {
		[INNATE_STAT_STRENGTH] = "Strength",
		[INNATE_STAT_MIGHT] = "Might",
		[INNATE_STAT_DEXTERITY] = "Dexterity",
		[INNATE_STAT_AGILITY] = "Agility",
		[INNATE_STAT_STAMINA] = "Stamina",
		[INNATE_STAT_PRECISION] = "Precision",
		[INNATE_STAT_HARDINESS] = "Hardiness",
		[INNATE_STAT_INTELLECT] = "Intellect",
		[INNATE_STAT_INTUITION] = "Intuition",
		[INNATE_STAT_SPIRIT] = "Spirit",
		[INNATE_STAT_WILL] = "Will",
		[INNATE_STAT_RESOLVE] = "Resolve",
		[INNATE_STAT_WISDOM] = "Wisdom",
		[INNATE_STAT_LETHALITY] = "Lethality"
})
--]]
Global ("RD", {
	ver = {common.GetAddonName(),5, "30/10/2012","Nikon","Unit Detector"},
	ids = {},	--ID`s tables // 1-friends 2-enemies 3-mobs 4-devices
	obj = {{},{},{},{}},	--Objects`s tables // 1-friends 2-enemies 3-crown 4-searched
	maxTbl = {10,10,1,1},
	searched = common.GetEmptyWString(),		--search WString
	timer = Sets.rTime,
	upTime = Sets.rTime,
	unitsChanged = true,
	block = true,
	blockscan = true,
	targetid = nil,
	scanedid = nil,
	oldLen = 0,
	deltaLen = 4,
	deltaX = 1,
	ul = {},--ul = {{},{}},
	stbl = {}
})
--[[
local cf = {
		[INNATE_STAT_STRENGTH] = 4,
		[INNATE_STAT_MIGHT] = 4,
		[INNATE_STAT_DEXTERITY] = 4,
		[INNATE_STAT_AGILITY] = 1,
		[INNATE_STAT_STAMINA] = 1,
		[INNATE_STAT_PRECISION] = 4,
		[INNATE_STAT_HARDINESS] = 1,
		[INNATE_STAT_INTELLECT] = 4,
		[INNATE_STAT_INTUITION] = 4,
		[INNATE_STAT_SPIRIT] = 4,
		[INNATE_STAT_WILL] = 1,
		[INNATE_STAT_RESOLVE] = 1,
		[INNATE_STAT_WISDOM] = 0,
		[INNATE_STAT_LETHALITY] = 1
}
--]]
function RD:chkidlist()
--local cnt = 1
self.ul[2] = {}
--while self.ul[1][cnt] do
for _,t in pairs(self.ul[1]) do
	if t > 0 and object.IsExist(t) then table.insert(self.ul[2],t)
	else t = 0
	end
end



end

function RD:UpUnitList(tbl)
	if Sets.uDMetod then
		return;
	end
	
	local cnt = 1;
	
	while self.ul[cnt] do
		if object.IsExist(self.ul[cnt]) then
			cnt = cnt + 1;
		else
			table.remove(self.ul,cnt);
		end
	end
	--common.LogInfo("", "ul0 = ", tostring(table.getn(self.ul)))
	for _,t in pairs(tbl) do
		--self.ul[1][t] = t
		table.insert(self.ul,t);
	end
	--self:chkidlist()
	--common.LogInfo("", "ul = ", tostring(table.getn(self.ul)))
end

function RD:GetUnitList()
--return self.ul[2]
return self.ul
end

function RD:UpdLen ()
if self.targetid then
	if object.IsExist(self.targetid) then
		local mlen = self:GetLen (object.GetPos(self.targetid),avatar.GetPos())
		if mlen and math.abs(mlen - self.oldLen) > self.deltaLen then
			--common.LogInfo("","len - ", tostring(mlen))
			
			--self.oldLen = mlen
			--self.oldLen = math.floor(mlen - math.mod(mlen,self.deltaLen))-- * self.deltaLen
			self.oldLen = mlen - (math.fmod or math.mod)(mlen,self.deltaLen)
			--common.LogInfo("","len - ", tostring(self.oldLen))
		end
	else
		self.targetid = nil;
	end
end
end

function RD:UpdAngle (params)
if self.targetid then
	--local mang = self:GetAngle (object.GetPos(self.targetid),avatar.GetPos(),mission.GetCameraDirection())
	--local mang = self:GetAngle (object.GetPos(self.targetid),avatar.GetPos(),avatar.GetDir())
	local mang = self:GetAngle (object.GetPos(self.targetid),avatar.GetPos(),params)
	--if mlen and math.abs(mlen - self.oldLen) > self.deltaLen then
		--common.LogInfo("","len - ", tostring(mlen))
		
		--self.oldLen = mlen
		--self.oldLen = math.floor(mlen - math.mod(mlen,self.deltaLen))-- * self.deltaLen
		--self.oldLen = mlen - math.mod(mlen,self.deltaLen)
		--common.LogInfo("","ang - ", tostring(mang))
	--end
end
end


function RD:GetLen (pos1,pos2)
if pos1 and pos2 then
	--common.LogInfo("","x1 - ", tostring(pos1.posX)," x2 - ", tostring(pos2.posX))
	--common.LogInfo("","y1 - ", tostring(pos1.posY)," x2 - ", tostring(pos2.posY))
	--common.LogInfo("","z1 - ", tostring(pos1.posZ)," x2 - ", tostring(pos2.posZ))
	
	return math.sqrt((pos1.posX-pos2.posX)^2 + (pos1.posY-pos2.posY)^2 + (pos1.posZ-pos2.posZ)^2)
	
	
end
end

function RD:GetAngle (unp,avp,ang)
if unp and avp and ang then
	--common.LogInfo("","!! - ", tostring(180*ang/math.pi))
	--common.LogInfo("","y1 - ", tostring(pos1.posY)," x2 - ", tostring(pos2.posY))
	--common.LogInfo("","z1 - ", tostring(math.sqrt(unp.posX^2 + unp.posY^2))," x2 - ", tostring(unp.posX-unp.posY)," ang - ", tostring(ang))
	
	--return math.sqrt((pos1.posX-pos2.posX)^2 + (pos1.posY-pos2.posY)^2 + (pos1.posZ-pos2.posZ)^2)
	--return 180*(math.acos((-unp.posX-0*unp.posY)/math.sqrt(unp.posX^2 + unp.posY^2))-ang)/math.pi-180
	--return 180*(math.acos((avp.posY-unp.posY)/math.sqrt((unp.posX-avp.posX)^2 + (unp.posY-avp.posY)^2))-ang)/math.pi
	local ac = {}
	ac.posX = avp.posX/math.cos(ang)
	ac.posY = avp.posY/math.cos(ang)
	--common.LogInfo("","y1 - ", tostring(ac.posY)," x1 - ", tostring(ac.posY))
	--common.LogInfo("","y0 - ", tostring(avp.posY)," x0 - ", tostring(avp.posY))
	
	local p1 = (avp.posX - unp.posX)^2 + (avp.posY - unp.posY)^2 + (avp.posX - ac.posX)^2 + (avp.posY - ac.posY)^2 + (unp.posX - ac.posX)^2 + (unp.posY - ac.posY)^2
	local p2 = 2 * math.sqrt((avp.posX - unp.posX)^2 + (avp.posY - unp.posY)^2)
	local p3 = math.sqrt((avp.posX - ac.posX)^2 + (avp.posY - ac.posY)^2)
	
	--local p1 = (avp.posX - ac.posX)*(unp.posX - avp.posX) + (avp.posY - ac.posY)*(unp.posY - avp.posY)
	--local p2 = math.sqrt((avp.posX - ac.posX)^2 * (avp.posY - ac.posY)^2)
	--local p3 = math.sqrt((unp.posX - avp.posX)^2 * (unp.posY - avp.posY)^2)
	
	--return 180*(math.acos((avp.posX-unp.posX)/math.sqrt((unp.posX-avp.posX)^2 + (unp.posY-avp.posY)^2))+ang)/math.pi
	return math.acos(p1/(p2*p3))
	
end
end
function RD:StartScan(id)
end

--[[
function RD:StartScan (id)
if id and object.IsExist(id) then
	avatar.EndInspect()
	--common.LogInfo("","RD:StartScan()")
	self.scanedid = id
	avatar.StartInspect(id)
end
end

function RD:Scaning ()
local sm = {0,0,{0,0,0,0,0,0},""}
if self.scanedid then
	--common.LogInfo("","Scaning ",object.GetName(self.scanedid))
	
	
	--for i = ITEM_CONT_EQUIPMENT,ITEM_CONT_EQUIPMENT_RITUAL do
	local i=ITEM_CONT_EQUIPMENT
		for v, t in pairs(unit.GetEquipmentItemIds(self.scanedid,i)) do
			if avatar.GetItemInfo(t).runeInfo then
				sm[3][1-DRESS_SLOT_OFFENSIVERUNE1+v] = avatar.GetItemInfo(t).runeInfo.runeLevel
			else
				--common.LogInfo("",avatar.GetItemInfo(t).name, (avatar.GetItemInfo(t).isWeapon and " isWeapon" or ""))
				if avatar.GetItemBonus(t).innateStats then
					for u,p in pairs(avatar.GetItemBonus(t).innateStats) do
					
					--common.LogInfo("", tostring(v),"- ",tostring(p.base)," - ",tostring(p.effective)," - ",tostring(p.equipment)," - ",tostring(p.enchants)," - ",tostring(p.talents))
					
						if p.effective > 0 then
							sm[2] = sm[2] + p.effective * (cf[u]==1 and 1 or 0)
							sm[1] = sm[1] + p.effective * ((v >= DRESS_SLOT_MAINHAND) and (v <= DRESS_SLOT_RANGED) and cf[u] or 1)
							--sm[1] = sm[1] + p.effective * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and 4 or 1)
						--if p.base > 0 then
							--sm[2] = sm[2] + p.base * (cf[u]==1 and 1 or 0)
							--sm[1] = sm[1] + p.base * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and cf[u] or 1)
						--elseif i == ITEM_CONT_EQUIPMENT_RITUAL then
							--sm[1] = sm[1] + (avatar.GetItemInfo(t).level < 50 and .4 or .8)
						end
					end
				end
			end
		end
	--end
	
	i=ITEM_CONT_EQUIPMENT_RITUAL
		for v, t in pairs(unit.GetEquipmentItemIds(self.scanedid,i)) do
			--if avatar.GetItemInfo(t).runeInfo then
				--sm[3][1-DRESS_SLOT_OFFENSIVERUNE1+v] = avatar.GetItemInfo(t).runeInfo.runeLevel
			--else
				--common.LogInfo("",avatar.GetItemInfo(t).name, (avatar.GetItemInfo(t).isWeapon and " isWeapon" or ""))
				if (v >= DRESS_SLOT_MAINHAND) and (v <= DRESS_SLOT_RANGED) then
				if avatar.GetItemBonus(t).innateStats then
					for u,p in pairs(avatar.GetItemBonus(t).innateStats) do
					
					--common.LogInfo("", tostring(v),"- ",tostring(p.base)," - ",tostring(p.effective)," - ",tostring(p.equipment)," - ",tostring(p.enchants)," - ",tostring(p.talents))
					
						if p.effective > 0 then
							sm[2] = sm[2] + p.effective * (cf[u]==1 and 1 or 0)
							sm[1] = sm[1] + p.effective * ((v >= DRESS_SLOT_MAINHAND) and (v <= DRESS_SLOT_RANGED) and cf[u] or 1)
							--sm[1] = sm[1] + p.effective * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and 4 or 1)
						--if p.base > 0 then
							--sm[2] = sm[2] + p.base * (cf[u]==1 and 1 or 0)
							--sm[1] = sm[1] + p.base * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and cf[u] or 1)
						--elseif i == ITEM_CONT_EQUIPMENT_RITUAL then
							--sm[1] = sm[1] + (avatar.GetItemInfo(t).level < 50 and .4 or .8)
						end
					end
				end
				else
					--common.LogInfo("", tostring(sm[1]))
					sm[1] = sm[1] + 6 + (avatar.GetItemInfo(t).level > 50 and 2 or 0)
					--common.LogInfo("", "- ",tostring(sm[1]))
				end
			--end
		end
--]]	
	--[[for i = ITEM_CONT_EQUIPMENT,ITEM_CONT_EQUIPMENT_RITUAL do
		for v, t in pairs(unit.GetEquipmentItemIds(self.scanedid,i)) do
			if avatar.GetItemInfo(t).runeInfo then
				sm[3][1-DRESS_SLOT_OFFENSIVERUNE1+v] = avatar.GetItemInfo(t).runeInfo.runeLevel
			else
				--common.LogInfo("",avatar.GetItemInfo(t).name, (avatar.GetItemInfo(t).isWeapon and " isWeapon" or ""))
				if avatar.GetItemBonus(t).innateStats then
					for u,p in pairs(avatar.GetItemBonus(t).innateStats) do
					
					common.LogInfo("", tostring(v),"- ",tostring(p.base)," - ",tostring(p.effective)," - ",tostring(p.equipment)," - ",tostring(p.enchants)," - ",tostring(p.talents))
					
						if p.effective > 0 then
							sm[2] = sm[2] + p.effective * (cf[u]==1 and 1 or 0)
							sm[1] = sm[1] + p.effective * ((v >= DRESS_SLOT_MAINHAND) and (v <= DRESS_SLOT_RANGED) and cf[u] or 1)
							--sm[1] = sm[1] + p.effective * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and 4 or 1)
						--if p.base > 0 then
							--sm[2] = sm[2] + p.base * (cf[u]==1 and 1 or 0)
							--sm[1] = sm[1] + p.base * (v >= DRESS_SLOT_MAINHAND and v <= DRESS_SLOT_RANGED and cf[u] or 1)
						--elseif i == ITEM_CONT_EQUIPMENT_RITUAL then
							--sm[1] = sm[1] + (avatar.GetItemInfo(t).level < 50 and .4 or .8)
						end
					end
				end
			end
		end
	end]]
--[[
	--avatar.EndInspect()
	--common.LogInfo("",object.GetName(self.scanedid), " points - ",tostring(sm[1] - sm[2]), " / ",tostring(sm[2]), " / ",tostring(sm[1]))
	for i=1,table.getn(sm[3]) do
		--common.LogInfo("",tostring(sm[3][i]))
		sm[4] = sm[4] .. tostring(sm[3][i]) .. " "
	end
	sm[3] = object.GetName(self.scanedid)
	self.scanedid = nil
	sm[1] = math.floor(sm[1])
	--avatar.EndInspect()
else
	sm[1] = -1
end
return sm
end
--]]
function RD:Selfscan()
--self.scanedid = avatar.GetId()
--return self:Scaning()
--GS.RequestInfo(self.scanedid)
end


function RD:SetBlock (onoff)
self.block = onoff
--self.timer = onoff and Sets.rTime or self.timer
self.timer = onoff and self.upTime or self.timer
return onoff
end

function RD:DetectCh(params)
	self.unitsChanged = true;
	if not self.blockscan and params then
		if params.spawned then
			self:ObjSearch(params.spawned);
		elseif params.unitId and params.unitId == self.obj[4].id then
			self:ObjRescan();
		end
	end
end

function RD:setcolor (num,id,quality)
if object.IsDead(id) then return 1 end
if quality then return quality == UNIT_QUALITY_BOSS and 7 or 6 end
return (unit.GetPvPFlagInfo(id).isOn and 1 or 0) + num * 2
end

function RD:addid (id)
--common.LogInfo("", "id = ", tostring(id))
if not object.IsExist(id) then
	--self.ul[1] = {}
	--self.ul = {}
	return
end
if unit.IsPlayer(id) then
	if object.IsFriend(id) then table.insert(self.ids[1],id)
	else table.insert(self.ids[2],id)
	end
elseif not unit.IsPet(id) then table.insert(self.ids[3],id)
end
end

function RD:GetidList ()
for i=1,3 do self.ids[i] = {} end
local tbl = Sets.uDMetod and avatar.GetUnitList() or self:GetUnitList()
--common.LogInfo("", "tbl = ", tostring(table.getn(tbl)))
if not tbl then return end
--common.LogInfo("", "tbl!!! ")
for v,t in pairs(tbl) do
	--common.LogInfo("", "t = ", tostring(t))
	self:addid(t)
	
end
end

function RD:ObjRescan()
	if self.blockscan then
		return;
	end
	
	self.unitsChanged = true;
	self.obj[4].id = nil;
	--self:ObjSearch (avatar.GetUnitList())
	self:ObjSearch(Sets.uDMetod and avatar.GetUnitList() or self:GetUnitList());
	--self:ObjSearch (self:GetUnitList())
	if not self.obj[4].id then
		self:ObjSearch(avatar.GetDeviceList());
	end
end

function RD:ObjSearch (tbl)
	if self.obj[4].id or self.blockscan then
		return; 
	end
	
	local nam
	
	for v,t in pairs(tbl) do
		nam = object.GetName(t);
		if common.IsSubstringEx(nam, self.searched)
			and (not object.IsUnit(t) and true or not object.IsDead(t))
		then
			self.obj[4].id = t;
			self.obj[4].name = nam;
			--self.block = false
			--if not avatar.GetTarget() and (unit.GetHealthPercentage(t)<20) then avatar.SelectTarget(t) end
			self.unitsChanged = true;
			--object.Highlight( t, "AMBIENT", {r=0.5,g=0,b=1,a=0.1}, {r=1,g=0,b=0,a=1.0}, 3)
			--if object.IsExist(t) then
				object.Highlight( t, "AMBIENT", {r=1,g=0,b=0,a=1.0}, {r=1,g=1,b=1,a=1.0}, 5)
			--end
			--break
		end
	end
end

--[[
function s(t)
	if not unit.GetGuildInfo(t) then
		return false
	end
	if common.IsSubstring(unit.GetGuildInfo(t).name, userMods.ToWString("Ударная Волна")) then
		return true
	else
		return false
	end
end
]]--

function s(t)
	--[[
	local class = unit.GetClass(t).className
	if class == "NECROMANCER" then 
		return true
	else
		return false
	end
	]]--
	if common.IsSubstring(object.GetName( t ) or userMods.ToWString(""), userMods.ToWString("Кот")) then 
		return true
	end
	return false
	

	
	--[[
	--if not unit.GetGuildInfo(avatar.GetId()) then
		local guildInfo = unit.GetGuildInfo( t )
		local name = guildInfo and guildInfo.name
		if common.IsSubstring(name or userMods.ToWString(""), userMods.ToWString(sGuild)) then 
			if guildInfo and guildInfo.rank < 3 then
				return true
			end
		end
		-- return false
	--end
	return false
	]]--
	
	--[[
	
	if not unit.GetGuildInfo(t) then
		return false
	end
	local guildInfoTarget = unit.GetGuildInfo( t )
	local nameTarget = guildInfoTarget and guildInfoTarget.name
	local guildInfo = unit.GetGuildInfo( avatar.GetId() )
	local name = guildInfo and guildInfo.name
	if common.IsSubstring(nameTarget or userMods.ToWString(""), name or userMods.ToWString("")) then
		return true
	end
	return false	
	
	]]--
end

function SortOrder(a, b)
	local aLevel = unit.GetLevel(a.id)
	local bLevel = unit.GetLevel(b.id)
	local aScore = unit.GetGearScore( a.id )
	local bScore = unit.GetGearScore( b.id )
	if aLevel > bLevel then
		return true
	elseif aLevel == bLevel then
		if aScore > bScore then
			return true
		else
			return false
		end
	else
		return false
	end
end

function RD:GetObjects ()
--local tt = {mission.GetLocalTimeHMS(),0}
local tbl, n = {}
for i=1,2 do self.obj[i] = {} end
self:GetidList()
for i=1,2 do
	for v,t in pairs(self.ids[i]) do
		if button_state == 0 or button_state == 1 and s(t) then
			--avatar.SelectTarget(t)
			tbl = {}
			tbl.id = t
			--LogInfo(object.GetName(t))
			if button_state == 1 then
				common.LogInfo( "common", userMods.FromWString(object.GetName(t)).." "..userMods.FromWString(unit.GetGuildInfo( t ).name) )
			end
			tbl.level = unit.GetLevel(t)
			tbl.className = unit.GetClass(t)
			tbl.className, tbl.clasLocal = tbl.className.className, tbl.className.name
			--tbl.colorText = unit.GetPrimaryTarget(t)==avatar.GetId() and 1 or 0
			tbl.colorText = 0
			tbl.color = self:setcolor (i,t)
			table.insert(self.obj[i],tbl)
		end
	end
end
n = table.getn(self.obj[1]) + table.getn(self.obj[2])
self.upTime = Sets.rTime * (n < 11 and 1 or (n < 31 and 2 or (n < 51 and 3 or 4)))
--self.upTime = Sets.rTime * (n < 11 and 2 or (n < 31 and 4 or (n < 51 and 6 or 8)))
--common.LogInfo(common.GetAddonName(),"n = ",tostring(n)," upTime ",tostring(self.upTime))

--Sort
for i=1, 2 do
	if self.obj[i][2] then
		if Sets.onSort then
			--table.sort(self.obj[i], SortOrder)
			table.sort(self.obj[i], function(a, b) return (1e5*a.level+1e10*a.colorText-a.id) > (1e5*b.level+1e10*b.colorText-b.id)end)
		else
			--table.sort(self.obj[i], function(a, b) return (a.id) < (b.id)end)
		end
	end
end

--elite
self.obj[3].id = nil
for v,t in pairs(self.ids[3]) do
	n = unit.GetQuality(t)
	--if ((n == UNIT_QUALITY_BOSS) or (n == UNIT_QUALITY_FLAVOR_ELITE)) and not object.IsDead(t) then
	if n == UNIT_QUALITY_FLAVOR_ELITE and not object.IsDead(t) then
	--if n == UNIT_QUALITY_CRITTER and not object.IsDead(t) then
		self.obj[3].id = t
		self.obj[3].level = unit.GetLevel(t)
		self.obj[3].className = "MOB"
		self.obj[3].colorText = 0
		self.obj[3].color = self:setcolor (3,t,n)
		--self.block = false
		self.unitsChanged = true
		break
	end
end
--searched
if self.obj[4].id then
	self.obj[4].id = object.IsExist(self.obj[4].id) and self.obj[4].id or nil
	if not self.obj[4].id then self:ObjRescan () end
end
end

function RD:Update ()
if self.block then return end
--if self.timer < Sets.rTime then
if self.timer < self.upTime then
	self.timer = self.timer+1
else
	self.timer = 1
	if self.unitsChanged then
		self:GetObjects ()
		self.unitsChanged = false
		return true
	end
end
end

function RD:count (numtbl)
--return table.getn(self.ids[numtbl==1 and 1 or 2])
return table.getn(self.obj[numtbl==1 and 1 or 2])
end

function RD:Init ()
self.oldLen = 0
self.targetid = nil
self.block = false
self.unitsChanged = true
self.upTime = Sets.rTime
self.blockscan = string.len(userMods.FromWString(self.searched)) < 1
self.obj[4].id = not self.blockscan and self.obj[4].id or nil
self:ObjRescan ()
self.stbl = self:Selfscan()
end