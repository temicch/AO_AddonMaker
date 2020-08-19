local wtChat = nil
local GlobalFontSize = 14

local function LocateChat()
	if not wtChat then
		local w = stateMainForm:GetChildUnchecked("ChatLog", false)
		if not w then
			w = stateMainForm:GetChildUnchecked("Chat", true)
		else
			w = w:GetChildUnchecked("Container", true) -- w = w:GetChildUnchecked("Chat", true);
		end
		wtChat = w
	end
	return wtChat;
end
function Chat(message, color, fontSize)
	wtChat = LocateChat();
	if (not wtChat) then
		LogInfo("No chat");
		return;
	end;

	local valuedText = common.CreateValuedText()
	local format = "<body alignx='left' fontname='AllodsWest' fontsize='"..(fontSize or GlobalFontSize).."' shadow='1' ><rs class='color'><r name='text'/></rs></body>"
	color = color and color or "LogColorYellow"
	if not common.IsWString( message ) then 
		message = userMods.ToWString(message) 
	end
	common.SetTextValues(valuedText, {
	format = userMods.ToWString(format),
	text = message,
	color = color
	})	
	wtChat:PushFrontValuedText( valuedText )
end

function ChatItem(itemId, fontSize)
	wtChat = LocateChat()
	if (not wtChat) then
		LogInfo("No chat")
		return
	end

	local valuedText = common.CreateValuedText()
	local format = "<body alignx='left' fontname='AllodsWest' fontsize='"..(fontSize or GlobalFontSize).."' shadow='1' ><rs class='color'>[<Cursed><r name='icon'/>]</Cursed></rs> <tip_white><r name='text'/></tip_white></body>"
	common.SetTextValues(valuedText, {
	format = userMods.ToWString(format),
	--name = itemLib.GetItemInfo( itemId ).name,
	icon = itemLib.GetValuedObject( itemId ),
	text = userMods.ToWString(FromMillisecondsToString(itemLib.GetTemporaryInfo( itemId ).remainingMs, true)),
	color = COLORS[itemLib.GetQuality( itemId ).quality]
	})
	wtChat:PushFrontValuedText( valuedText )
end