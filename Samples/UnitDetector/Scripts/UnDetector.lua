if not unit.GetHealthPercentage then unit.GetHealthPercentage = function (id) return object.GetHealthInfo(id).valuePercents end end
local Numbers = {"I","II","III","IV","V","VI","VII","VIII","IX","X"}
local Pth = {
	{alignX = WIDGET_ALIGN_CENTER,
	alignY = WIDGET_ALIGN_LOW,
	posX = 0,
	posY = 200,
	highPosX = 0,
	highPosY = 0,
	sizeX = 700,
	sizeY = 100},
	{
	
	}
	
	
	
	
}
Global("wt", {
	--wtControl3D = stateMainForm:GetChildUnchecked( "MainAddonMainForm", false ):GetChildChecked("MainScreenControl3D", false),
	--wtControl3D2 = mainForm:GetChildUnchecked( "W3D", false),
	Buttons = {{},{}},
	ButtonInfo = mainForm:GetChildUnchecked("ButtonM", false),
	ButtonElite = nil,		--"E"
	ButtonSearch = nil,		--"S"
	Tooltip = {mainForm:GetChildUnchecked("PanelTT", false),nil},
	PanelUnit = mainForm:GetChildUnchecked("PanelUnit", false),
	PanelPt = mainForm:GetChildUnchecked("PanelPt", false),
	TextPt = {mainForm:GetChildUnchecked("PanelPt", false):GetChildUnchecked("Text", false),nil},
	TextM = mainForm:GetChildUnchecked("TextM", false),
	TextB = nil,
	LineEdit = nil,
	oldTarget = nil,
	AT = {{},{}}
	--AT3 = {{},{}}
})


function wt:ptsShow (tbl,tbl2)
if tbl[1]>0 then
	--Inspected
	self.TextPt[1]:SetVal("atk",common.FormatInt(tbl[1]-tbl[2],"%02d"))
	self.TextPt[1]:SetVal("def",common.FormatInt(tbl[2],"%02d"))
	self.TextPt[1]:SetVal("pts",common.FormatInt(tbl[1],"%02d"))
	self.TextPt[1]:SetVal("nick",tbl[3])
	self.TextPt[1]:SetVal("rne",userMods.ToWString(tbl[4]))
	--Self
	self.TextPt[2]:SetVal("atk",common.FormatInt(tbl2[1]-tbl2[2],"%02d"))
	self.TextPt[2]:SetVal("def",common.FormatInt(tbl2[2],"%02d"))
	self.TextPt[2]:SetVal("pts",common.FormatInt(tbl2[1],"%02d"))
	self.TextPt[2]:SetVal("nick",tbl2[3])
	self.TextPt[2]:SetVal("rne",userMods.ToWString(tbl2[4]))
	
	self.PanelPt:Show(true)
end
end

function wt:nS (flg)
--common.LogInfo("","nS (flg)")
self.LineEdit:Show(false)
self.AT[1].id = flg and self.AT[1].id or nil
--self.AT[1].wdt:Show(flg and self.AT[1].id and object.IsUnit(self.AT[1].id) or false)
--self:reTarget()
end

--[[function wt:reTarget ()
local trgt = avatar.GetTarget()
self.oldTarget = trgt or self.oldTarget
--common.LogInfo("","oldTarget ",tostring(self.oldTarget), " GetTarget ",tostring(avatar.GetTarget()))
for i=2,1,-1 do
	if self.AT[i].id and object.IsExist(self.AT[i].id) then
		if object.IsUnit(self.AT[i].id) then
				--self.wtControl3D2:Show(true)
				--common.LogInfo("","wtControl3D2:Show(true)")
			--common.LogInfo("","AT ",tostring(self.AT[i].id))
			self.AT[i].wdt:Show((not self.oldTarget or self.AT[i].id==self.oldTarget) or false)
			if not trgt and self.oldTarget~=self.AT[1].id and self.oldTarget~=self.AT[2].id then
				if not object.IsDead(self.AT[i].id) then avatar.SelectTarget(self.AT[i].id) end
				--break
			end
		elseif object.IsDevice(self.AT[i].id) then
			self.AT[i].wdt:Show(false)
		end
	else
		self.AT[i].wdt:Show(false)
		self.AT[i].id = nil
		--self.wtControl3D2:Show(false)
		--common.LogInfo("","wtControl3D2:Show(false)")
	end
end
end]]


function wt:Attach2D (id,num)
--num == 1(Search) or 2(Crown)

--[[if id and object.IsDetected(id) then
	object.AttachWidget2D( id, self.AT[1].wdt, ATTACHED_OBJECT_POS_UP )
	
	
end]]


num = num ~= 1 and 2 or 1
--common.LogInfo("","Attach2D")
if not avatar.GetTarget() and object.IsUnit(id) then
	self.oldTarget = nil
	--self.wtControl3D2:Show(true)
	---common.LogInfo("","wtControl3D2:Show(true) ",tostring(Sets.uDMetod))
	--if Sets.uDMetod then avatar.SelectTarget(id) end
	--mission.SetCharacterScene( CHAR_SCENE_ITEMMALL, self.wtControl3D2, id )
	--mission.SetCharacterSceneScaleFactor( CHAR_SCENE_ITEMMALL, 0.25 )
	--mission.RotateCharacterScene( CHAR_SCENE_ITEMMALL, 6 )
	--self.AT[num].wdt:PlayResizeEffect( self.AT[num].p1, self.AT[num].p2, 3e3, EA_SYMMETRIC_FLASH )
--else
	--common.LogInfo("","AttachWidget3D")
	--object.AttachWidget3D(id, self.wtControl3D, self.AT3[num].wdt, -5 )
end

--object.AttachWidget3D(id, self.wtControl3D, self.AT3[num].wdt, 1 )

self.AT[num].id = id
--object.Highlight(id, "AMBIENT", {r=0.5,g=0,b=1,a=0.1}, {r=1,g=0,b=0,a=1.0}, 3)
end

function wt:CreateWidgets()
--units
local x, y, pos, wtButDesc
pos = self.PanelUnit:GetPlacementPlain()
if Sets.Place == "L" then
	y = widgetsSystem:GetPosConverterParams().fullVirtualSizeY - Sets.Top - Sets.Bottom
	pos.sizeY, pos.sizeX, pos.posY, pos.posX = y, 24, Sets.Top, 0
else
	y = widgetsSystem:GetPosConverterParams().fullVirtualSizeX - Sets.Top - Sets.Bottom
	pos.sizeY, pos.sizeX, pos.posY, pos.posX = 24, y, 0, Sets.Top
end
self.PanelUnit:SetPlacementPlain(pos)
y = (y - 24) / 24
RD.maxTbl[1] = math.floor(y/2)
RD.maxTbl[2] = math.floor(y/2)
Sets.Direct = Sets.Direct == "T>B" and 1 or -1

x,y = 1,1
repeat
	y = 1
	if (x==1) and (y==1) then
		self.Buttons[1][1] = self.PanelUnit:GetChildUnchecked("ButtonC", false)
		pos = self.Buttons[1][1]:GetPlacementPlain()
		wtButDesc = self.Buttons[1][1]:GetWidgetDesc()
	end
	repeat
		self.Buttons[x][y] = mainForm:CreateWidgetByDesc(wtButDesc)
		if Sets.Place == "L" then
			pos.posY = 24*(RD.maxTbl[1] + Sets.Direct*(x>1 and y or (-y)))
		else
			pos.posX = 24*(RD.maxTbl[1] + Sets.Direct*(x>1 and y or (-y)))
		end
		self.PanelUnit:AddChild(self.Buttons[x][y])
		self.Buttons[x][y]:SetPlacementPlain(pos)
		self.Buttons[x][y]:SetName("U_"..tostring(y))
		y = y + 1
	until y > RD.maxTbl[x]
	x = x + 1
until x>2

--Elite button
self.ButtonElite = mainForm:CreateWidgetByDesc(wtButDesc)
pos.posX, pos.posY = 0, 24
self.ButtonElite:SetPlacementPlain(pos)
self.ButtonElite:SetName("E_0")
self.ButtonElite:SetBackgroundTexture(common.GetAddonRelatedTexture("ELITE"))
--Search button
self.ButtonSearch = mainForm:CreateWidgetByDesc(wtButDesc)
if Sets.Place == "L" then
	pos.posY, pos.posX = 24*RD.maxTbl[1], 0
else
	pos.posY, pos.posX = 0, 24*RD.maxTbl[1]
end
self.ButtonSearch:SetPlacementPlain(pos)
self.PanelUnit:AddChild(self.ButtonSearch)
self.ButtonSearch:SetName("S_sss")
self.ButtonSearch:SetBackgroundTexture(common.GetAddonRelatedTexture("SEARCH"))
self.ButtonSearch:SetBackgroundColor(sColor[8])
self.ButtonSearch:Show(true)

--Editline
self.LineEdit = mainForm:GetChildUnchecked("WEL", false)
x = self.LineEdit:GetPlacementPlain()
if Sets.Place == "L" then
	x.posX = pos.posX + pos.sizeX
	x.posY = Sets.Top + pos.posY - x.sizeY
else
	x.posY = pos.posY + pos.sizeY
	x.posX = Sets.Top + pos.posX - x.sizeX
end
self.LineEdit:SetPlacementPlain(x)

--Attach2D
---------

self.AT[1].wdt = mainForm:GetChildUnchecked("Mark", false)
self.AT[1].wdt:SetBackgroundColor(sColor[13])
self.AT[1].wdt:SetForegroundColor(sColor[14])
self.AT[1].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKSB"))
--self.AT[2].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKCB"))
self.AT[1].wdt:SetForegroundTexture(common.GetAddonRelatedTexture("MARKSF"))
self.AT[1].wdt:Show(false)
self.AT[1].idt = nil

self.AT[1].p1, self.AT[1].p2 = self.AT[1].wdt:GetPlacementPlain(), {}
	self.AT[1].p1.sizeX, self.AT[1].p1.sizeY = 64, 64
	--self.AT[i].p1.sizeX, self.AT[i].p1.sizeY = 64, 64
	self.AT[1].p2.sizeX = self.AT[1].p1.sizeX*2
	self.AT[1].p2.sizeY = self.AT[1].p2.sizeX

self.AT[2].wdt = mainForm:CreateWidgetByDesc(self.AT[1].wdt:GetWidgetDesc())

--[[local wtDonor = stateMainForm:GetChildUnchecked("ContextDamageVisualization",false):GetChildUnchecked("EnemyPanel",false)--:GetChildUnchecked("EnemyDmgPanel01",false)
--local wParasite = mainForm:GetChildChecked( "Target" , false )
--wHost:AddChild(wParasite)
----------
wtButDesc = mainForm:GetChildUnchecked("Mark", false):GetWidgetDesc()
for i=1, 2 do
	self.AT[i].wdt = mainForm:CreateWidgetByDesc(wtButDesc)
	
	--[ [self.AT3[i].wdt = mainForm:CreateWidgetByDesc(wtButDesc)
	self.AT3[i].wdt:SetBackgroundColor(sColor[13])
	self.AT3[i].wdt:SetForegroundColor(sColor[14])
	self.AT3[i].wdt:Show(true)] ]
	
	self.AT[i].wdt:SetBackgroundColor(sColor[13])
	self.AT[i].wdt:SetForegroundColor(sColor[14])
	self.AT[i].wdt:Show(false)
	
	wtDonor:AddChild(self.AT[i].wdt)-------------------
	
	self.AT[i].p1, self.AT[i].p2 = mainForm:GetChildUnchecked("Mark", false):GetPlacementPlain(), {}
	--for v,t in pairs(self.AT[i].p1) do self.AT[i].p2.v = t end
	self.AT[i].p1.sizeX, self.AT[i].p1.sizeY = i*32, i*32
	--self.AT[i].p1.sizeX, self.AT[i].p1.sizeY = 64, 64
	self.AT[i].p2.sizeX = self.AT[i].p1.sizeX*2
	self.AT[i].p2.sizeY = self.AT[i].p2.sizeX
	
		--self.wtControl3D:AddWidget3D( self.AT3[i].wdt, {sizeX = 1, sizeY = 1}, avatar.GetPos()--[ [{posX = 0, posY = 0, posZ = 0}] ], true, false, 100, WIDGET_3D_BIND_POINT_LOW, 1, 10 )
		

		
		
	--self.wtControl3D:AddWidget3D( self.AT[i].wdt, {sizeX = 1000, sizeY = 1000}, avatar.GetPos()--[ [{posX = 0, posY = 0, posZ = 0}] ], false, false, 100, WIDGET_3D_BIND_POINT_CENTER, .5, 50 )
end
self.AT[1].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKSB"))
self.AT[2].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKCB"))
self.AT[1].wdt:SetForegroundTexture(common.GetAddonRelatedTexture("MARKSF"))
self.AT[2].wdt:SetForegroundTexture(common.GetAddonRelatedTexture("MARKCF"))]]

--[[self.AT3[1].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKSB"))
self.AT3[2].wdt:SetBackgroundTexture(common.GetAddonRelatedTexture("MARKCB"))
self.AT3[1].wdt:SetForegroundTexture(common.GetAddonRelatedTexture("MARKSF"))
self.AT3[2].wdt:SetForegroundTexture(common.GetAddonRelatedTexture("MARKCF"))]]


--pos = object.GetPos(avatar.GetId())
--pos.posY,pos.posZ,pos.posX = pos.posY + 0, pos.posZ +5,pos.posX +5
--pos.posY = pos.posY + 5
--common.LogInfo("","SetWidget3DPos ",tostring(pos.posY)," ",tostring(pos.posZ)," ",tostring(pos.posX))
--object.AttachWidget3D(avatar.GetId(), self.wtControl3D, self.AT[1].wdt, 3 )

--self.wtControl3D:SetWidget3DPos(self.AT[1].wdt, pos )





--[[pos = widgetsSystem:GetPosConverterParams()
x,y = pos.realSizeX,pos.realSizeY
pos = self.wtControl3D2:GetPlacementPlain()
pos.alignX,pos.alignY = WIDGET_ALIGN_HIGH, WIDGET_ALIGN_LOW
pos.highPosX,pos.posY = 50, 200
pos.sizeX,pos.sizeY  = 150,150
self.wtControl3D2:SetPlacementPlain(pos)]]

--Tooltip
self.Tooltip[2] = self.Tooltip[1]:GetChildUnchecked("WTC", false)
self.Ticon = {self.Tooltip[1]:GetChildUnchecked("Panel", false)}
self.Ticon[2] = mainForm:CreateWidgetByDesc(self.Ticon[1]:GetWidgetDesc())
self.Tooltip[1]:AddChild(self.Ticon[2])
x = self.Ticon[1]:GetPlacementPlain()
x.posY = x.posY + x.sizeY + 10
self.Ticon[2]:SetPlacementPlain(x)
self.Ticon[3] = mainForm:CreateWidgetByDesc(self.Ticon[1]:GetWidgetDesc())
self.Tooltip[1]:AddChild(self.Ticon[3])
x.posY,x.sizeX,x.sizeY = x.posY - 7,280,4
self.Ticon[3]:SetPlacementPlain(x)
wt.Ticon[3]:SetBackgroundColor(sColor[1])
self.Ticon[3] = mainForm:CreateWidgetByDesc(self.Ticon[1]:GetWidgetDesc())
self.Tooltip[1]:AddChild(self.Ticon[3])
x.sizeX = 200
self.Ticon[3]:SetPlacementPlain(x)
wt.Ticon[3]:SetBackgroundColor(sColor[11])
wt.Ticon[3]:SetPriority(550)

self.Txt = {self.Tooltip[1]:GetChildUnchecked("Text", false)}
for i=2,4 do
	self.Txt[i] = mainForm:CreateWidgetByDesc(self.Txt[1]:GetWidgetDesc())
	self.Tooltip[1]:AddChild(self.Txt[i])
end
x = self.Txt[1]:GetPlacementPlain()
x.sizingX = WIDGET_SIZING_DEFAULT
x.posX,x.posY,x.sizeY = 50,50,20
self.Txt[2]:SetPlacementPlain(x)
x.posX,x.posY,x.sizeY = 50,68,20
self.Txt[3]:SetPlacementPlain(x)
x.posX,x.highPosX,x.posY,x.sizeY,x.alignX = 5,5,38,18,WIDGET_ALIGN_CENTER
self.Txt[4]:SetPlacementPlain(x)
--self.Txt[4]:SetFormat(userMods.ToWString("<html fontsize='12' outline ='1'><tip_blue><r name='text'/> %</tip_blue></html>"))
self.Txt[4]:SetFormat(userMods.ToWString("<html fontsize='12' outline ='1'><r name='text'/> %</html>"))
self.Txt[4]:SetPriority(750)
self.Tooltip[1]:Show(false)

--Points text
self.TextPt[2] = mainForm:CreateWidgetByDesc(self.TextPt[1]:GetWidgetDesc())
self.PanelPt:AddChild(self.TextPt[2])
pos = self.TextPt[1]:GetPlacementPlain()
--pos.posX = math.ceil(self.PanelPt:GetPlacementPlain().sizeX/2 - 5)
pos.posX = math.ceil(self.PanelPt:GetPlacementPlain().sizeX/2 - 5)
pos.alignX = WIDGET_ALIGN_LOW
self.TextPt[2]:SetPlacementPlain(pos)

--Points text in "InspectCharacter" panel
self.TextB = mainForm:CreateWidgetByDesc(self.TextPt[1]:GetWidgetDesc())
self.TextB:SetFormat(userMods.ToWString("<html fontsize='14' outline ='1'><r name='text'/></html>"))
pos = self.TextB:GetPlacementPlain()
pos.posX, pos.posY = 280,Sets.uDMetod and 360 or 320
self.TextB:SetPlacementPlain(pos)
--common.LogInfo("", "Sets.uDMetod ", tostring(Sets.uDMetod))
end


function wt:goCSW(flg)
local plc = {self.AT[2].wdt:GetPlacementPlain(),self.AT[2].wdt:GetPlacementPlain(),
			self.AT[2].wdt:GetPlacementPlain(),self.AT[2].wdt:GetPlacementPlain()}
--plc[2] = plc[1]
plc[1].posX, plc[1].posY = 50,100
plc[2].posX, plc[2].posY = 620,800
--plc[2].alignX, plc[2].alignY = WIDGET_ALIGN_CENTER,WIDGET_ALIGN_CENTER
plc[3].sizeX, plc[3].sizeY = 100,100


self.AT[1].wdt:Show(flg)
if flg then
	self.AT[1].wdt:PlayMoveEffect(plc[1], plc[2], 1000, EA_MONOTONOUS_INCREASE)
	self.AT[1].wdt:PlayResizeEffect(plc[1], plc[3], 2000, EA_MONOTONOUS_INCREASE)
	self.AT[1].wdt:PlayFadeEffect(1, 0, 3000,EA_SYMMETRIC_FLASH)
end

end

function wt:myTooltip(wdt)
if wdt then
	local pos = {wdt:GetPlacementPlain(), self.Tooltip[1]:GetPlacementPlain(),self.PanelPt:GetPlacementPlain()}
	if Sets.Place == "L" then
		pos[2].posX = 2*pos[1].sizeX + pos[1].posX
		pos[2].posY = (string.find(wdt:GetName(),"E_") and 0 or Sets.Top) + pos[1].posY
		pos[3].posX = pos[2].posX +pos[2].sizeX --+ 20
		pos[3].posY = pos[2].posY
	else
		pos[2].posY = (string.find(wdt:GetName(),"E_") and 0 or 2*pos[1].sizeY) + pos[1].posY
		pos[2].posX = (string.find(wdt:GetName(),"E_") and 2*pos[1].sizeX or Sets.Top) + pos[1].posX
	end
	self.Tooltip[1]:SetPlacementPlain(pos[2])
	self.PanelPt:SetPlacementPlain(pos[3])
end
self.Tooltip[1]:Show(RD:SetBlock(wdt~=nil))
end

function wt:Update()
--Units
--common.LogInfo(common.GetAddonName(),"wt:Update()")
self.ButtonInfo:SetVal("friends",common.FormatInt(RD:count(1),"%02d"))
self.ButtonInfo:SetVal("enemies",common.FormatInt(RD:count(2),"%02d"))
for i=1,2 do
		for v,t in pairs(self.Buttons[i]) do
			t:Show(RD.obj[i][v]~=nil)
			if RD.obj[i][v] then
				t:SetName("U_"..tostring(RD.obj[i][v].id))
				t:ClearValues()
				if RD.obj[i][v].colorText > 0 then
					t:SetVal("rlevel",common.FormatInt(RD.obj[i][v].level,"%d"))
				else
					t:SetVal("wlevel",common.FormatInt(RD.obj[i][v].level,"%d"))
				end
				t:SetBackgroundTexture(common.GetAddonRelatedTexture(RD.obj[i][v].className))
				t:SetBackgroundColor(sColor[RD.obj[i][v].color])
			end
		end
end
--Elite
if RD.obj[3].id and object.IsExist(RD.obj[3].id) then
	if self.ButtonElite:GetName()~= ("E_"..tostring(RD.obj[3].id)) then
		self.ButtonElite:SetName("E_"..tostring(RD.obj[3].id))
		self.ButtonElite:SetVal("wlevel",common.FormatInt(RD.obj[3].level,"%d"))
		self.ButtonElite:SetBackgroundColor(sColor[RD.obj[3].color])
		--if RD.obj[3].id~=RD.obj[4].id then
			--common.LogInfo("","Attach2D 2")
			self:Attach2D(RD.obj[3].id,2)
		--else
			
		--end
	end
--[[elseif self.AT[2].id and object.IsExist(self.AT[2].id) then
	--object.DetachWidget2D(self.AT[2].id)
	self.AT[2].id = nil]]
	--self.AT[2].wdt:Show(false)
end

self.ButtonElite:Show(RD.obj[3].id~=nil)
--Find
self.ButtonSearch:SetBackgroundColor(sColor[RD.obj[4].id and 9 or 8])
if RD.obj[4].id and object.IsExist(RD.obj[4].id) then
	if self.ButtonSearch:GetName()~= ("S_"..tostring(RD.obj[4].id)) then
		self.ButtonSearch:SetName("S_"..tostring(RD.obj[4].id))
		--if RD.obj[3].id~=RD.obj[4].id then
			--common.LogInfo("","Attach2D 3")
			self:Attach2D(RD.obj[4].id,1)
		--else
			
		--end
	end
--[[elseif self.AT[1].id and object.IsExist(self.AT[1].id) then
	--object.DetachWidget2D(self.AT[1].id)
	self.AT[1].id = nil]]
	--self.AT[1].wdt:Show(false)
end
end

function wt:SetTTH(num)
local pos = self.Tooltip[1]:GetPlacementPlain()
pos.sizeY = num
self.Tooltip[1]:SetPlacementPlain(pos)
end

function wt:SetMyParents()
--Set parent of my Mark (min priority)
local Par = stateMainForm:GetChildUnchecked("MainAddonMainForm",false)
--[[if Par then
	Par:AddChild(self.AT[1].wdt)
	Par:AddChild(self.AT[2].wdt)
end]]
--common.LogInfo(common.GetAddonName(),"AT.wdt Parent:", Par:GetName())
--Set parent of my Form (middle priority)
Par = stateMainForm:GetChildUnchecked("ContextBag",false)
if Par then
	Par:AddChild(mainForm)
end
--common.LogInfo(common.GetAddonName(),"mainForm Parent:", Par:GetName())
--Set parent of my Tooltip (max priority)
Par = stateMainForm:GetChildUnchecked("ContextTooltip",false)
if Par then
	Par:AddChild(self.Tooltip[1])
	Par:AddChild(self.PanelPt)
else
	Par = stateMainForm:GetChildUnchecked("Tooltip",true)
	if Par then Par:GetParent():AddChild(self.Tooltip[1]) end
end

Par = stateMainForm:GetChildUnchecked("InspectCharacter",false):GetChildUnchecked("MainPanel",false)
if Par then
	Par:AddChild(self.TextB)
else self.TextB:Show(false)
	--common.LogInfo(common.GetAddonName(),"no:")
end

--common.LogInfo(common.GetAddonName(),"Tooltip Parent:", Par:GetName())
end

function wt:EnableUP(flg)
if flg then
	RD:DetectCh ()
	self.ButtonInfo:SetVal("friends",common.FormatInt(RD:count(1),"%02d"))
	self.ButtonInfo:SetVal("enemies",common.FormatInt(RD:count(2),"%02d"))
	RegEv(EVRe)
	--common.RegisterEventHandler(EVRe.EVENT_SECOND_TIMER,"EVENT_SECOND_TIMER")
else
	UnRegEv(EVRe)
	--common.UnRegisterEventHandler(EVRe.EVENT_SECOND_TIMER,"EVENT_SECOND_TIMER")
	self.ButtonInfo:SetVal("friends",userMods.ToWString("--"))
	self.ButtonInfo:SetVal("enemies",userMods.ToWString("--"))
	--[[for i=1,2 do
		if self.AT[i].id then
			--object.DetachWidget2D(self.AT[i].id)
			--self.AT[i].wdt:Show(false)
		end
	end]]
end
self.PanelUnit:Show(flg)
end

function wt:Init()
self:CreateWidgets()
self:SetMyParents()
end

---=====----
function ReCountLenMark()
RD:UpdLen()

end

function SetHPbar(HPpers)
wt.Ticon[3]:Show(HPpers and true or false)
wt.Txt[4]:Show(HPpers and true or false)
if not HPpers then return end
local pos = wt.Ticon[3]:GetPlacementPlain()
pos.sizeX = HPpers * (wt.Ticon[3]:GetParent():GetPlacementPlain().sizeX - 20) / 100
wt.Ticon[3]:SetPlacementPlain(pos)
wt.Ticon[3]:SetBackgroundColor(sColor[HPpers<25 and 10 or (HPpers>70 and 11 or 12)])
wt.Txt[4]:SetVal("text",common.FormatInt(HPpers,"%d"))
end

function Repaint()
--local mCor = coroutine.create(function () if RD:Update() then wt:Update() end end)
--coroutine.resume(mCor)
--coroutine.resume(coroutine.create(function () if RD:Update() then wt:Update() end end))
if RD:Update() then wt:Update() end
end

function GetTextLocalized (strText)
return Lc[Sets.Lang][strText] and userMods.ToWString(Lc[Sets.Lang][strText]) or userMods.ToWString(strText)
end

function GetRank(rank)
	if 0 == rank then
		return "Лидер"
	elseif 1 == rank then
		return "Казначей"
	elseif 2 == rank then
		return "Офицер"
	elseif 3 == rank then
		return "Ветеран"
	elseif 4 == rank then
		return "Старшина"
	elseif 5 == rank then
		return "Рядовой"
	elseif 6 == rank then
		return "Дублер"
	elseif 7 == rank then
		return "Новобранец"
	elseif 8 == rank then
		return "Штрафник"
	elseif 9 == rank then
		return "Изгой"
	end
end

function PrepareTT (w)
local id = tonumber(string.sub(w:GetName(),3))
local vT, HH, tmp, tip = common.CreateValuedText(), 91
if string.find(w:GetName(),"U_") then	--Player info
	if not object.IsExist(id) then return end
	--id = avatar.GetId()
	tmp = {
		Class = unit.GetClass(id),
		Great = unit.IsGreat(id),
		FI = unit.GetFairyInfo(id),
		Fic = unit.GetFairyZodiacSignInfo(id),
		Guild = unit.GetGuildName and {level = 0,name = unit.GetGuildName(id)} or unit.GetGuildInfo(id),
		--HPP = unit.GetHealthPercentage and unit.GetHealthPercentage(id) or (object.GetHealthInfo(id) or {}).valuePercents,
		HPP = (object.GetHealthInfo(id) or {}).valuePercents,
		Level = common.FormatInt(unit.GetLevel(id),"%d"),
		Name = object.GetName(id),
		Sex = unit.GetSex(id),
		RemtMN = remort and remort.GetMainName(id),
		Remt = remort and remort.IsAlt(id),
		vRank = unit.GetVeteranRank and unit.GetVeteranRank(id)
	}
	SetHPbar(tmp.HPP)
	wt.Tooltip[2]:RemoveItems()
--Class Icon
	wt.Ticon[1]:SetBackgroundColor(classColor[tmp.Class.className])
	wt.Ticon[1]:SetBackgroundTexture(common.GetAddonRelatedTexture(tmp.Class.className))
	wt.Ticon[1]:Show(true)
--Name
	tip = tmp.Remt and " color = '0xf000f0f0'" or ""
	wt.Txt[1]:SetFormat(userMods.ToWString("<header fontsize='18'"..tip..">[<r name='text2'/>] <r name='text'/><br/></header>"))
	wt.Txt[1]:SetVal("text", tmp.Name)
	if avatar.IsInspectAllowed() then
		wt.Txt[1]:SetVal("text2", common.FormatNumber( unit.GetGearScore( id ), "A4"))
	else
		wt.Txt[1]:SetVal("text2", "?")
	end
	--unit.GetGearScore( id )
	--tmp.Name..
--Info
	vT:SetFormat(userMods.ToWString("<html fontsize='14'><r name='race'/>, <r name='class'/>, <r name='level'/></html>"))
	vT:SetVal("race", tmp.Sex.raceSexName)
	vT:SetVal("class", tmp.Great and tmp.Class.greatName or tmp.Class.raceClassName)
	--vT:SetVal("class", tmp.Class.raceClassName)
	--vT:SetVal("class", tmp.Class.greatName)
	vT:SetVal("level", tmp.Level)
	wt.Tooltip[2]:PushBackValuedText(vT)
	HH = HH + 24
--Remort Info
	if tmp.Remt then 
		vT:SetFormat(userMods.ToWString("<header fontsize='14'><tip_green><r name='remr'/>: </tip_green><r name='rname'/></header>"))
		vT:SetVal("rname", tmp.RemtMN)
		tip = tmp.RemtMN and "Remort_MN" or "NA"
		vT:SetVal("remr", GetTextLocalized(tip))
		wt.Tooltip[2]:PushBackValuedText(vT)
		HH = HH + 24
	end
--Guild
	--if string.len(userMods.FromWString(tmp.Guild)) > 0 then
	if tmp.Guild and not common.IsEmptyWString(tmp.Guild.name) then 
		vT:SetFormat( userMods.ToWString("<html fontsize='16' color = '0xf0f000f0'><r name='text'/>, <r name='lvl'/></html>"))
		---vT:SetFormat( userMods.ToWString("<html fontsize='16' color = '0xf0f000f0'><r name='text'/>, <r name='lvl'/> <r name='loclvl'/></html>"))
		local GuildName = tmp.Guild.name
		vT:SetVal( "text", tmp.Guild.name)
		vT:SetVal( "lvl", common.FormatInt(tmp.Guild.level,"%d"))
		wt.Tooltip[2]:PushBackValuedText(vT)
		vT:SetFormat( userMods.ToWString("<html fontsize='16' color = '0xf0f000f0'><tip_golden><r name='rank'/></tip_golden></html>"))
		vT:SetVal( "rank", userMods.ToWString(GetRank(tmp.Guild.rank)))
		wt.Tooltip[2]:PushBackValuedText(vT)
		
		vT:SetFormat( userMods.ToWString("<html fontsize='16' color = '0xf0f000f0'></html>"))
		wt.Tooltip[2]:PushBackValuedText(vT)
		HH = HH + 24 + 24 + 24
	end
--Fairy Info
	if tmp.FI.isExist and Sets.SFairy then
		wt.Ticon[2]:SetBackgroundTexture(tmp.Fic.image)
		tip = tmp.FI.isHungry and "0xf0a0a0a0" or "0xf0f0f000"
		wt.Txt[2]:SetFormat( userMods.ToWString("<html fontsize='14' color = '"..tip.."'><r name='text'/>. <r name='rank'/> - <r name='level'/></html>"))
		tip = tmp.FI.isHungry and {r = 0.5; g = 0.5; b = 0.5; a = 1.0} or {r = 1; g = 1; b = 1; a = 1.0}
		wt.Ticon[2]:SetBackgroundColor(tip)
		wt.Txt[2]:SetVal("text", tmp.Fic.name)
		wt.Txt[2]:SetVal("rank", userMods.ToWString(Numbers[tmp.FI.rank]))
		wt.Txt[2]:SetVal("level", common.FormatInt(tmp.FI.level,"%d"))
		wt.Txt[3]:SetFormat( userMods.ToWString("<html fontsize='14'><r name='stat'/> +<r name='bonus'/></html>"))
		--wt.Txt[3]:SetVal("stat", GetTextLocalized(IS[tmp.FI.bonusStat]))
		--wt.Txt[3]:SetVal("bonus", common.FormatInt(tmp.FI.bonusStatValue,"%d"))
		wt.Txt[3]:SetVal("stat", GetTextLocalized("Power"))
		wt.Txt[3]:SetVal("bonus", common.FormatInt(tmp.FI.powerBonus,"%d"))
		wt.Ticon[2]:Show(true)
		wt.Txt[2]:Show(true)
		wt.Txt[3]:Show(true)
	else
		wt.Ticon[2]:Show(false)
		wt.Txt[2]:Show(false)
		wt.Txt[3]:Show(false)
	end
--Veteran Info
	if tmp.vRank then 
		vT:SetFormat(userMods.ToWString("<header fontsize='14'><r name='value'/></header>"))
		vT:SetVal("value", tmp.vRank.name)
		wt.Tooltip[2]:PushBackValuedText(vT)
		HH = HH + 24
	end
	wt:SetTTH(HH)
	return true
elseif string.find(w:GetName(),"S_") then	--Find object info
--Icon
	wt.Ticon[1]:SetBackgroundColor(sColor[RD.obj[4].id and 9 or 8])
	wt.Ticon[1]:SetBackgroundTexture(common.GetAddonRelatedTexture("SEARCH"))
	wt.Ticon[1]:Show(true)
	wt.Ticon[2]:SetBackgroundColor(sColor[RD.obj[4].id and 9 or 8])
	wt.Ticon[2]:SetBackgroundTexture(common.GetAddonRelatedTexture("SEARCH"))
	wt.Ticon[2]:Show(true)
	SetHPbar()
	wt.Tooltip[2]:RemoveItems()
	--wt.PanelPt:Show(false)
--Header
	wt.Txt[1]:SetFormat(userMods.ToWString("<header alignx='center' fontsize='16'><r name='text'/><br/></header>"))
	wt.Txt[1]:SetVal("text", GetTextLocalized("TTIOT"))
--Search string
	wt.Txt[2]:SetFormat(userMods.ToWString("<html fontsize='14'><r name='text'/> :</html>"))
	wt.Txt[2]:SetVal("text", GetTextLocalized("Search"))
	wt.Txt[3]:SetFormat(userMods.ToWString("<html color = '0xf0f000f0' fontsize='16'><r name='text'/></html>"))
	wt.Txt[3]:SetVal("text", RD.blockscan and GetTextLocalized("None") or RD.searched)
--Find object string
	if RD.obj[4].id then
		vT:SetFormat(userMods.ToWString("<html fontsize='14'><r name='text'/> :</html>"))
		vT:SetVal("text", GetTextLocalized("Find"))
		wt.Tooltip[2]:PushBackValuedText(vT)
		vT:SetFormat(userMods.ToWString("<html color = '0xf0f000f0' fontsize='16'><r name='text'/></html>"))
		vT:SetVal("text", RD.obj[4].name)
		wt.Tooltip[2]:PushBackValuedText(vT)
		HH = HH + 44
	end
	wt:SetTTH(HH)
	return true
elseif string.find(w:GetName(),"E_") then	--Elite info
	if not object.IsExist(id) then return end
	tmp = {
		HPP = unit.GetHealthPercentage(id),
		Level = common.FormatInt(unit.GetLevel(id),"%d"),
		Title = unit.GetTitle(id),
		Name = object.GetName(id),
	}
--Icon
	wt.Ticon[1]:SetBackgroundColor(sColor[RD.obj[3].color])
	wt.Ticon[1]:SetBackgroundTexture(common.GetAddonRelatedTexture("ELITE"))
	wt.Ticon[1]:Show(true)
	wt.Ticon[2]:SetBackgroundColor(sColor[RD.obj[3].color])
	wt.Ticon[2]:SetBackgroundTexture(common.GetAddonRelatedTexture("ELITE"))
	wt.Ticon[2]:Show(true)
	SetHPbar(tmp.HPP)
	wt.Tooltip[2]:RemoveItems()
--Name
	wt.Txt[1]:SetFormat(userMods.ToWString("<header fontsize='16'><r name='text'/></header>"))
	wt.Txt[1]:SetVal("text", tmp.Name)
--Info
	wt.Txt[2]:SetFormat(userMods.ToWString("<html fontsize='14'><r name='level'/> <r name='nlevel'/></html>"))
	wt.Txt[2]:SetVal("level", tmp.Level)
	wt.Txt[2]:SetVal("nlevel", GetTextLocalized("Level"))
	wt.Txt[3]:SetFormat( userMods.ToWString("<html fontsize='14'><r name='value'/></html>"))
	wt.Txt[3]:SetVal("value", tmp.Title)
	wt:SetTTH(HH)
	return true
end
end

function SetAddonLoc (loc)
--Based on AO game Localization detection by SLA. Version 2011-02-10.
if loc == "auto" then
	for _,mb in pairs(cartographer.GetMapBlocks()) do
		for l,t in pairs({rus="\203\232\227\224", eng="Holy Land", ger="Heiliges Land", fra="Terre Sacr\233e", br="Terra Sagrada", jpn="\131\74\131\106\131\65"}) do
			if userMods.FromWString( cartographer.GetMapBlockInfo(mb).name ) == t then
				return SetAddonLoc(l)
			end
		end
	end
else
	for v,t in pairs(Lc) do
		if t.LShort and t.LShort == loc then return v end
	end
end
return 1
end

Global("button_off", { r = 1; g = 0; b = 0; a = 1 })
Global("button_on", { r = 0; g = 1; b = 0; a = 1 })
Global("button_state", 0)

function Init()
Reg(EVRe)
DnDInit()
--mainForm:GetChildChecked("ShowHideBtn", false):SetVal("val", "UD")	
mainForm:GetChildChecked("ShowHideBtn", false):SetTextColor(nil, button_off )
Dnd.Init(mainForm:GetChildChecked("ShowHideBtn", false), nil, true)
Dnd.Init(wt.ButtonInfo, nil, true)
if avatar.IsExist () then EVRe.EVENT_AVATAR_CREATED() end
end

Init()