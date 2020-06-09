Global("Locales", { })
Global("localization", "eng_eu")
local supportedLocales = {'eng_eu','fra','ger','rus','tr'}
local addonPath = string.sub(common.GetAddonInfo().sysFullName, 11)
for i,v in pairs(supportedLocales) do
	local path = "../data/Mods/Addons/"..addonPath.."/Scripts/Locales/Locales["..v.."].lua"
	if pcall(loadfile(path)) then
		local f = assert(loadfile(path))
		f()
	else
		common.LogInfo("","File for '"..v.."' localization does not exist. Please create the file Locales["..v.."] in the Locales folder, based on the file Locales[eng_eu]")
	end
end
-- +----------------------------------+
-- |AO game Localization detection    |
-- |New conceptual detection by Ciuine|
-- |Aesthetically improved by Ramirez |
-- +----------------------------------+

local function GetGameLocalization()
	local id = options.GetOptionsByCustomType( "interface_option_localization" )[ 0 ]
	if id then
		local values = options.GetOptionInfo( id ).values
		local value = values and values[ 0 ]
		local name = value and value.name
		if name then
			return userMods.FromWString( name )
		end
		return "eng_eu"
	end
	
end
localization = common.GetLocalization() or GetGameLocalization()

function GTL( strTextName )
	return Locales[ localization ][ strTextName ] or Locales[ "eng_eu" ][ strTextName ] or strTextName
end