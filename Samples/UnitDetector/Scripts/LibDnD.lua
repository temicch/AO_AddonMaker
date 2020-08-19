--------------------------------------------------------------------------------
-- LibDnd.lua // "Drag&Drop Library" by SLA, version 2011-05-28
--                                   updated version 2014-10-29 by hal.dll
-- Help, support and updates: 
-- https://alloder.pro/topic/260-how-to-libdndlua-biblioteka-dragdrop/
--------------------------------------------------------------------------------
Global( "Dnd", {} )
-- PUBLIC FUNCTIONS --
function Dnd.Init( wtMovable, wtReacting, fUseCfg, fLockedToParentArea, Padding, KbFlag, Cursor, oldParam1, oldParam2 )
	if wtMovable == Dnd then
		wtMovable, wtReacting, fUseCfg, fLockedToParentArea, Padding, KbFlag, Cursor, oldParam1 =
		           wtReacting, fUseCfg, fLockedToParentArea, Padding, KbFlag, Cursor, oldParam1, oldParam2
	end
	if type(wtMovable) == "number" then
		wtReacting, wtMovable, fUseCfg, fLockedToParentArea, Padding, KbFlag, Cursor, oldParam1 =
		           wtReacting, fUseCfg, fLockedToParentArea, Padding, KbFlag, Cursor, oldParam1, oldParam2
	end
	if type(wtMovable) ~= "userdata" then return end
	if not Dnd.Widgets then
		Dnd.Widgets = {}
		Dnd.Screen = widgetsSystem:GetPosConverterParams()
		common.RegisterEventHandler( Dnd.OnPickAttempt, "EVENT_DND_PICK_ATTEMPT" )
		common.RegisterEventHandler( Dnd.OnResolutionChanged, "EVENT_POS_CONVERTER_CHANGED" )
	end
	wtReacting = wtReacting or wtMovable
	local ID = Dnd.AllocateDndID(wtReacting)
	Dnd.Widgets[ ID ] = {}
	Dnd.Widgets[ ID ].wtReacting = wtReacting
	Dnd.Widgets[ ID ].wtMovable = wtMovable
	Dnd.Widgets[ ID ].Enabled = true
	Dnd.Widgets[ ID ].fUseCfg = fUseCfg or false
	Dnd.Widgets[ ID ].CfgName = fUseCfg and "Dnd:" .. Dnd.GetWidgetTreePath( Dnd.Widgets[ ID ].wtMovable )
	Dnd.Widgets[ ID ].fLockedToParentArea = fLockedToParentArea == nil and true or fLockedToParentArea
	Dnd.Widgets[ ID ].KbFlag = type(KbFlag) == "number" and KbFlag or false
	Dnd.Widgets[ ID ].Cursor = Cursor == false and "default" or type(Cursor) == "string" and Cursor or "drag"
	Dnd.Widgets[ ID ].Padding = { 0, 0, 0, 0 } -- { T, R, B, L }
	if type( Padding ) == "table" then
		for i = 1, 4 do
			if Padding[ i ] then
				Dnd.Widgets[ ID ].Padding[ i ] = Padding[ i ]
			end
		end
	elseif type( Padding ) == "number" then
		for i = 1, 4 do
			Dnd.Widgets[ ID ].Padding[ i ] = Padding
		end
	end
	local InitialPlace = Dnd.Widgets[ ID ].wtMovable:GetPlacementPlain()
	if fUseCfg then
		local Cfg = GetConfig( Dnd.Widgets[ ID ].CfgName )
		if Cfg then
			local LimitMin, LimitMax = Dnd.PrepareLimits( ID, InitialPlace )
			InitialPlace.posX = Cfg.posX or InitialPlace.posX
			InitialPlace.posY = Cfg.posY or InitialPlace.posY
			InitialPlace.highPosX = Cfg.highPosX or InitialPlace.highPosX
			InitialPlace.highPosY = Cfg.highPosY or InitialPlace.highPosY
			Dnd.Widgets[ ID ].wtMovable:SetPlacementPlain( Dnd.NormalizePlacement( InitialPlace, LimitMin, LimitMax ) )
		end
	end
	Dnd.Widgets[ ID ].Initial = { X = InitialPlace.posX, Y = InitialPlace.posY, HX = InitialPlace.highPosX, HY = InitialPlace.highPosY }
	local mt = getmetatable( wtReacting )
	if not mt._Show then
		mt._Show = mt.Show
		mt.Show = function ( self, show )
			self:_Show( show ); Dnd.Register( self, show )
		end
	end
	Dnd.Register( wtReacting, true )
end
function Dnd.Remove( wtWidget, oldParam1 )
	if not Dnd.Widgets then return end
	if wtWidget == Dnd then wtWidget = oldParam1 end
	local ID = Dnd.GetWidgetID( wtWidget )
	if ID then
		Dnd.Enable( wtWidget, false )
		Dnd.Widgets[ ID ] = nil
	end
end
function Dnd.Enable( wtWidget, fEnable, oldParam1 )
	if not Dnd.Widgets then return end
	if wtWidget == Dnd then wtWidget, fEnable = fEnable, oldParam1 end
	local ID = Dnd.GetWidgetID( wtWidget )
	if ID and Dnd.Widgets[ ID ].Enabled ~= fEnable then
		Dnd.Widgets[ ID ].Enabled = fEnable
		Dnd.Register( wtWidget, fEnable )
	end
end
function Dnd.IsDragging()
	return Dnd.Dragging and true or false
end
-- FREE BONUS --
function GetConfig( name )
	local cfg = userMods.GetGlobalConfigSection( common.GetAddonName() )
	if not name then return cfg end
	return cfg and cfg[ name ]
end
function SetConfig( name, value )
	local cfg = userMods.GetGlobalConfigSection( common.GetAddonName() ) or {}
	if type( name ) == "table" then
		for i, v in pairs( name ) do cfg[ i ] = v end
	elseif name ~= nil then
		cfg[ name ] = value
	end
	userMods.SetGlobalConfigSection( common.GetAddonName(), cfg )
end
-- INTERNAL FUNCTIONS --
function Dnd.AllocateDndID( wtWidget )
	local BaseID = 300200
	return BaseID + common.RequestIntegerByInstanceId(wtWidget:GetInstanceId())
end
function Dnd.GetWidgetID( wtWidget )
	local WtId = wtWidget:GetInstanceId()
	for ID, W in pairs( Dnd.Widgets ) do
		if W.wtReacting:GetInstanceId() == WtId or W.wtMovable:GetInstanceId() == WtId then
			return ID
		end
	end
end
function Dnd.GetWidgetTreePath( wtWidget )
	local components = {}
	while wtWidget do
		table.insert( components, 1, wtWidget:GetName() )
		wtWidget = wtWidget:GetParent()
	end
	return table.concat( components, '.' )
end
function Dnd.Register( wtWidget, fRegister )
	if not Dnd.Widgets then return end
	local ID = Dnd.GetWidgetID( wtWidget )
	if ID then
		if fRegister and Dnd.Widgets[ ID ].Enabled and Dnd.Widgets[ ID ].wtReacting:IsVisible() then
			mission.DNDRegister( Dnd.Widgets[ ID ].wtReacting, ID, true )
		elseif not fRegister then
			if Dnd.Dragging == ID then
				mission.DNDCancelDrag()
				Dnd.OnDragCancelled()
			end
			mission.DNDUnregister( Dnd.Widgets[ ID ].wtReacting )
		end
	end
end
-----------------------------------------------------------------------------------------------------------
function Dnd.GetParentRealSize( Widget )
	local ParentSize = {}
	local ParentRect
	local parent = Widget:GetParent()
	if parent then
		ParentRect = parent:GetRealRect()
		ParentSize.sizeX = (ParentRect.x2 - ParentRect.x1) * Dnd.Screen.fullVirtualSizeX / Dnd.Screen.realSizeX
		ParentSize.sizeY = (ParentRect.y2 - ParentRect.y1) * Dnd.Screen.fullVirtualSizeY / Dnd.Screen.realSizeY
	else
		ParentRect = { x1 = 0, y1 = 0, x2 = Dnd.Screen.realSizeX, y2 = Dnd.Screen.realSizeY }
		ParentSize.sizeX = Dnd.Screen.fullVirtualSizeX
		ParentSize.sizeY = Dnd.Screen.fullVirtualSizeY
	end
	return ParentSize, ParentRect
end
function Dnd.NormalizePlacement( Place, LimitMin, LimitMax )
	local Opposite = { posX = "highPosX", posY = "highPosY", highPosX = "posX", highPosY = "posY"  }
	for k,v in pairs(LimitMax) do
		if Place[k] > v then
			Place[ Opposite[k] ] = Place[ Opposite[k] ] + Place[k] - v
			Place[k] = v
		end
	end
	for k,v in pairs(LimitMin) do
		if Place[k] < v then
			Place[ Opposite[k] ] = Place[ Opposite[k] ] + Place[k] - v
			Place[k] = v
		end
	end
	return Place
end
function Dnd.PrepareLimits( ID, Place )
	local LimitMin = {}
	local LimitMax = {}
	local ParentSize, ParentRect = Dnd.GetParentRealSize( Dnd.Widgets[ ID ].wtMovable )
	local Padding = Dnd.Widgets[ ID ].Padding
	Place = Place or Dnd.Widgets[ ID ].wtMovable:GetPlacementPlain()
	if Place.alignX == WIDGET_ALIGN_LOW then
		LimitMin.posX = Padding[ 4 ]
		LimitMax.posX = ParentSize.sizeX - Place.sizeX - Padding[ 2 ]
	elseif Place.alignX == WIDGET_ALIGN_HIGH then
		LimitMin.highPosX = Padding[ 2 ]
		LimitMax.highPosX = ParentSize.sizeX - Place.sizeX - Padding[ 4 ]
	elseif Place.alignX == WIDGET_ALIGN_CENTER then
		LimitMin.posX = Place.sizeX / 2 - ParentSize.sizeX / 2 + Padding[ 4 ]
		LimitMax.posX = ParentSize.sizeX / 2 - Place.sizeX / 2 - Padding[ 2 ]
	elseif Place.alignX == WIDGET_ALIGN_BOTH then
		LimitMin.posX = Padding[ 4 ]
		LimitMin.highPosX = Padding[ 2 ]
	elseif Place.alignX == WIDGET_ALIGN_LOW_ABS then
		LimitMin.posX = Padding[ 4 ] * Dnd.Screen.realSizeX / Dnd.Screen.fullVirtualSizeX
		LimitMax.posX = ( ParentSize.sizeX - Place.sizeX - Padding[ 2 ] ) * Dnd.Screen.realSizeX / Dnd.Screen.fullVirtualSizeX
	end
	if Place.alignY == WIDGET_ALIGN_LOW then
		LimitMin.posY = Padding[ 1 ]
		LimitMax.posY = ParentSize.sizeY - Place.sizeY - Padding[ 3 ]
	elseif Place.alignY == WIDGET_ALIGN_HIGH then
		LimitMin.highPosY = Padding[ 3 ]
		LimitMax.highPosY = ParentSize.sizeY - Place.sizeY - Padding[ 1 ]
	elseif Place.alignY == WIDGET_ALIGN_CENTER then
		LimitMin.posY = Place.sizeY / 2 - ParentSize.sizeY / 2 + Padding[ 1 ]
		LimitMax.posY = ParentSize.sizeY / 2 - Place.sizeY / 2 - Padding[ 3 ]
	elseif Place.alignY == WIDGET_ALIGN_BOTH then
		LimitMin.posY = Padding[ 1 ]
		LimitMin.highPosY = Padding[ 3 ]
	elseif Place.alignY == WIDGET_ALIGN_LOW_ABS then
		LimitMin.posY = Padding[ 1 ] * Dnd.Screen.realSizeY / Dnd.Screen.fullVirtualSizeY
		LimitMax.posY = (ParentSize.sizeY - Place.sizeY - Padding[ 3 ] ) * Dnd.Screen.realSizeY / Dnd.Screen.fullVirtualSizeY
	end
	return LimitMin, LimitMax
end
-----------------------------------------------------------------------------------------------------------
function Dnd.OnPickAttempt( params )
	local Picking = params.srcId
	if Dnd.Widgets[ Picking ] and Dnd.Widgets[ Picking ].Enabled and ( not Dnd.Widgets[ Picking ].KbFlag or Dnd.Widgets[ Picking ].KbFlag == KBF_NONE and params.kbFlags == KBF_NONE or common.GetBitAnd( params.kbFlags, Dnd.Widgets[ Picking ].KbFlag ) ~= 0 ) then
		Dnd.Place = Dnd.Widgets[ Picking ].wtMovable:GetPlacementPlain()
		Dnd.Reset = Dnd.Widgets[ Picking ].wtMovable:GetPlacementPlain()
		Dnd.Cursor = { X = params.posX , Y = params.posY }
		Dnd.Screen = widgetsSystem:GetPosConverterParams()
		if Dnd.Widgets[ Picking ].fLockedToParentArea then
			Dnd.LimitMin, Dnd.LimitMax = Dnd.PrepareLimits( Picking, Dnd.Place )
		end
		common.SetCursor( Dnd.Widgets[ Picking ].Cursor )
		Dnd.Dragging = Picking
		common.RegisterEventHandler( Dnd.OnDragTo, "EVENT_DND_DRAG_TO" )
		common.RegisterEventHandler( Dnd.OnDropAttempt, "EVENT_DND_DROP_ATTEMPT" )
		common.RegisterEventHandler( Dnd.OnDragCancelled, "EVENT_DND_DRAG_CANCELLED" )
		-- AO 2.0.06+ All IDs other than 14xxx and 15xxx need confirmation
		mission.DNDConfirmPickAttempt()
	end
end
function Dnd.OnDragTo( params )
	if not Dnd.Dragging then return end
	local dx = params.posX - Dnd.Cursor.X
	local dy = params.posY - Dnd.Cursor.Y
	if Dnd.Place.alignX ~= WIDGET_ALIGN_LOW_ABS then
		dx = dx * Dnd.Screen.fullVirtualSizeX / Dnd.Screen.realSizeX
	end
	if Dnd.Place.alignY ~= WIDGET_ALIGN_LOW_ABS then
		dy = dy * Dnd.Screen.fullVirtualSizeY / Dnd.Screen.realSizeY
	end
	Dnd.Place.posX = math.floor( Dnd.Reset.posX + dx )
	Dnd.Place.posY = math.floor( Dnd.Reset.posY + dy )
	Dnd.Place.highPosX = math.floor( Dnd.Reset.highPosX - dx )
	Dnd.Place.highPosY = math.floor( Dnd.Reset.highPosY - dy )
	if Dnd.Widgets[ Dnd.Dragging ].fLockedToParentArea then
		Dnd.Place = Dnd.NormalizePlacement( Dnd.Place, Dnd.LimitMin, Dnd.LimitMax )
	end
	Dnd.Widgets[ Dnd.Dragging ].wtMovable:SetPlacementPlain( Dnd.Place )
	common.SetCursor( Dnd.Widgets[ Dnd.Dragging ].Cursor )
end
function Dnd.OnDropAttempt()
	Dnd.StopDragging( true )
end
function Dnd.OnDragCancelled()
	Dnd.StopDragging( false )
end
function Dnd.StopDragging( fSuccess )
	if not Dnd.Dragging then return end
	common.UnRegisterEventHandler( Dnd.OnDragTo, "EVENT_DND_DRAG_TO" )
	common.UnRegisterEventHandler( Dnd.OnDropAttempt, "EVENT_DND_DROP_ATTEMPT" )
	common.UnRegisterEventHandler( Dnd.OnDragCancelled, "EVENT_DND_DRAG_CANCELLED" )
	if fSuccess then
		mission.DNDConfirmDropAttempt()
		if Dnd.Widgets[ Dnd.Dragging ].fUseCfg then
			SetConfig( Dnd.Widgets[ Dnd.Dragging ].CfgName, { posX = Dnd.Place.posX, posY = Dnd.Place.posY, highPosX = Dnd.Place.highPosX, highPosY = Dnd.Place.highPosY } )
		end
		Dnd.Widgets[ Dnd.Dragging ].Initial = { X = Dnd.Place.posX, Y = Dnd.Place.posY, HX = Dnd.Place.highPosX, HY = Dnd.Place.highPosY }
	else
		Dnd.Widgets[ Dnd.Dragging ].wtMovable:SetPlacementPlain( Dnd.Reset )
	end
	Dnd.Place = nil
	Dnd.Reset = nil
	Dnd.Cursor = nil
	Dnd.LimitMin = nil
	Dnd.LimitMax = nil
	Dnd.Dragging = nil
	common.SetCursor( "default" )
end
function Dnd.OnResolutionChanged()
	mission.DNDCancelDrag()
	Dnd.OnDragCancelled()
	Dnd.Screen = widgetsSystem:GetPosConverterParams()
	for ID, W in pairs( Dnd.Widgets ) do
		if W.fLockedToParentArea then
			local InitialPlace = W.wtMovable:GetPlacementPlain()
			local LimitMin, LimitMax = Dnd.PrepareLimits( ID, InitialPlace )
			InitialPlace.posX = W.Initial.X
			InitialPlace.posY = W.Initial.Y
			InitialPlace.highPosX = W.Initial.HX
			InitialPlace.highPosY = W.Initial.HY
			W.wtMovable:SetPlacementPlain( Dnd.NormalizePlacement( InitialPlace, LimitMin, LimitMax ) )
		end
	end
end