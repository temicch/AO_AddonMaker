--------------------------------------------------------------------------------
--- Timer Format
--------------------------------------------------------------------------------
function PadLeft(s, length, padstring)
  local result = s
  while string.len(result) < length do
    result = padstring .. result
  end
  return result
end

local formatTable = nil
formatTable = 
{
  hh = function(ms)
    return PadLeft(tostring(math.fmod(math.floor(ms / 3600000), 24)), 2, "0")
  end,
  mm = function(ms)
    return PadLeft(tostring(math.fmod(math.floor(math.round(ms / 1000)/ 60), 60)), 2, "0") 
  end,
  ss = function(ms)
    return PadLeft(tostring(math.fmod(math.floor(ms / 1000), 60)), 2, "0")
  end,
  fff = function(ms)
    return PadLeft(tostring(math.fmod(ms, 1000)) , 3, "0")
  end,
  ff = function(ms)
    return string.sub(formatTable["fff"](ms), 1, 2)
  end,
  f = function(ms)
    return string.sub(formatTable["fff"](ms), 1, 1)
  end,
  sc = function(ms)
    return PadLeft(tostring(math.fmod(math.ceil(ms / 1000), 60)), 2, "0")
  end
}
local formatTableOrder = { "hh", "mm", "ss", "fff", "ff", "f", "sc" }

local byte0 = string.byte("0", 1)
local byte9 = string.byte("9", 1)
function IsNumber(s)
  if string.len(s) > 0 then
    local firstbyte = string.byte(s, 1)
	return firstbyte >= byte0 and firstbyte <= byte9
  end
  return false
end

function FormatTimer(ms, format_str)
  if type(ms) ~= "number" then
    -- fix it: whats the reason that ms is sometimes a boolean?
    return ""
  end
  local result = format_str
  local isNegative = ms < 0
  ms = isNegative and (-ms) or ms
  for k, v in pairs( formatTableOrder ) do
    result = string.gsub(result, v, formatTable[v](ms))
  end  
  -- delete starting zeros
  while string.len(result) > 1 and string.sub(result, 1, 1) == "0" and IsNumber(string.sub(result, 2, 2)) do
    result = string.sub(result, 2, string.len(result))
  end
  if isNegative then
    result = (isNegative and "-" or "") .. result
  end
  return result
end
--------------------------------------------------------------------------------
--- INITIALIZATION
--------------------------------------------------------------------------------

--------------------------------------------------------------------------------

--------------------------------------------------------------------------------
