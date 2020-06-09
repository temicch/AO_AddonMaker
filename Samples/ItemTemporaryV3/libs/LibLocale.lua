local locale = common.GetLocalization()
local defaultLang = "rus"

function TXT(str)
	local _locale = locale
	if texts[_locale] == nil then
		_locale = defaultLang
	end
	if texts[_locale][str] == nil then
		_locale = defaultLang
	end
	if _locale == defaultLang and texts[defaultLang][str] == nil then
		return "string "..str.." doesn't exist"
	end
	return texts[_locale][str]
end
