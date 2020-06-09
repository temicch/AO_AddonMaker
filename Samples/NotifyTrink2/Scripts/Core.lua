-- +----------------+
-- |Helper functions|
-- +----------------+
function GetTableSize( t )
	if not t then
		return 0
	end
	local count = 0
	for _, _ in pairs(t) do
		count = count + 1
	end
	return count
end
-- +-----------------------+
-- |Remove value from table|
-- +-----------------------+
function RemoveValueFromTable(t, v)
for i, x in pairs(t) do
		if Compare(x,v) then
			table.remove(t,i)
		end
	end
end
-- +-------------------+
-- |Find Value in table|
-- +-------------------+
function isInTable(t,v)
for _, x in pairs(t) do
		if Compare(x,v) then
			return true
		end
	end
	return false
end
-- +--------------+
-- |Compare tables|
-- +--------------+
function Compare(t1, t2)
	if t1 == nil or t2 == nil then return false end
	if type(t1) ~= type(t2) then 
		return false
	end
	if type(t1) == "string" or type(t1) == "number" or type(t1) == "boolean" then
		if not (t1 == t2) then
			return false
		end
	else
		for i, _ in t1 do
			local type1 = type(t1[i])
			local type2 = type(t2[i])
			if type1 ~= type2 then 
				return false
			else
				if type1 == "string" or type1 == "number" or type1 == "boolean" then
					if t2[i] == nil or not (t1[i] == t2[i]) then 
						return false 
					end
				elseif type1 == "table" then
					if not Compare(t1[i], t2[i]) then 
						return false 
					end
				else
					if t1[i].IsEqual == nil then break
					else
						if not t1[i]:IsEqual(t2[i]) then 
							return false 
						end
					end
				end
			end
		end
		for i, _ in t2 do
			local type1 = type(t1[i])
			local type2 = type(t2[i])
			if type1 ~= type2 then 
				return false
			else
				if type2 == "string" or type2 == "number" or type2 == "boolean" then
					if t1[i] == nil or not (t2[i] == t1[i]) then 
					 	return false 
					end
				elseif type2 == "table" then
					if not Compare(t2[i], t1[i]) then 
						return false 
					end
				else
					if t2[i].IsEqual == nil then break
					else
						if not t2[i]:IsEqual(t1[i]) then 
							return false 
						end
					end
				end
			end
		end
	end
	return true
end
function printTable1(t)
for i, _ in pairs(t) do
		LogInfo(tostring(i)..":"..tostring(t[i]))
	end
end
function getTableName(t, tbl)
  for k, v in pairs(tbl) do
    if v == t then
          return k
    end
  end
  return nil
end

function printTable( t, nr, tbl )
	if type(t) ~= "table" or t == nil then 
		return false 
	end
	local tbl = tbl or _G
	local nr = nr or 0
	local tab = ""
	for _=0,nr do tab = tab.."  " end
	LogInfo(tab..tostring(getTableName(t,tbl)).." = {")
for i,v in pairs(t) do
		if type(v) == "string" then
			LogInfo(tab.."  "..tostring(i).." = "..v.."("..tostring(type(v))..")")
		elseif type(v) == "number" or type(v) == "boolean" then
			LogInfo(tab.."  "..tostring(i).." = "..tostring(v).."("..tostring(type(v))..")")
		elseif type(v) == "table" then
			printTable(v,nr+1, t)
		elseif type(v) == "userdata" then LogInfo("userdata")
		else LogInfo("not defined")
		end
	end
	LogInfo(tab.."}")
end

-- +-------------------------+
-- | Get text from game files|
-- +-------------------------+
function GetText(l)
	local VT = common.CreateValuedText()
	VT:SetFormat(userMods.ToWString("<html><t href='"..l.."'/></html>"))
	local text
	text = userMods.FromWString(common.ExtractWStringFromValuedText(VT))
	return text
end


-- +---------------------------+
-- |Event and Reaction Handlers|
-- +---------------------------+
function RegisterEventHandlers( handlers )
for event, handler in pairs(handlers) do
		common.RegisterEventHandler( handler, event )
	end
end

function RegisterReactionHandlers( handlers )
for event, handler in pairs(handlers) do
		common.RegisterReactionHandler( handler, event )
	end
end


-- +---------------------------------------+
-- |Shortcuts for WString/String conversion|
-- +---------------------------------------+
function toWS( arg )
	return userMods.ToWString(arg)
end

function fromWS( arg )
	return userMods.FromWString(arg)
end

-- +--------+
-- |Rounding|
-- +--------+
function round(val, decimal)
  if val == 0 then return 0 end
  local exp = decimal and 10^decimal or 1
  return math.ceil(val * exp - 0.5) / exp
end

function IsAddonActive(addonName)
	local addons = common.GetStateManagedAddons ()
for _,v in pairs(addons) do
		if v.name == "UserAddon/"..addonName and v.isLoaded then
			return true
		end
	end
	return false
end

function LogError ( text )
	LogToChat( text )
	LogInfo("", text )
end

function GetSettings( n )
	local settings = userMods.GetAvatarConfigSection( n )
	return settings
end

function SetSettings( p,s )
	userMods.SetAvatarConfigSection( p,s )
end
