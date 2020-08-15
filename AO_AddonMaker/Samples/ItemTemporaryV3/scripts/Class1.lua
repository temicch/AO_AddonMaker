Global("cMainPanel", {})
setmetatable(cMainPanel, {
	__call = function()
		local wtPanel = mainForm:GetChildChecked("panel", true)
		wtPanel:SetClipContent(false)
		DnD.Init( wtPanel, nil, true)
		
		local wtAddonName = mainForm:CreateWidgetByDesc(mainForm:GetChildChecked("Text2", true):GetWidgetDesc())
		local pl = wtAddonName:GetPlacementPlain()
		wtAddonName:	SetVal("val", common.GetAddonName())
		pl.alignX = 3
		pl.alignY = 0
		pl.posY = 10
		wtAddonName:	SetFormat("<header alignx = \"center\" fontsize=\"18\"><rs class=\"class\"><r name=\"val\"/></rs></header>")
		wtAddonName:SetPlacementPlain(pl)
		wtAddonName:Show(true)
		wtPanel:AddChild(wtAddonName)
		
		return wtPanel
	end
})