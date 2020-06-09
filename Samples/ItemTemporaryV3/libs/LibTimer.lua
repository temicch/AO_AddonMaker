-- +=================================+
-- |LibTimer                         |
-- |Emulate a simple timer function  |
-- |Author: Zurion/Cristi Mirt       |
-- |Version: 1.0.0                   |
-- |Last update: 01-11-2016          |
-- +=================================+

Global("timerWidgetDesc",nil)
Global("timerDesc", {})

function InitTimer0()
	local wtAddonMainForm = mainForm
	if wtAddonMainForm ~= nil then
		local wtChild = wtAddonMainForm:GetNamedChildren()
		if table.maxn(wtChild) > 0 then
			local newTimer = mainForm:CreateWidgetByDesc(wtChild[0]:GetWidgetDesc())
			newTimer:Show(false)
			timerWidgetDesc = newTimer:GetWidgetDesc()
		end
	end
end

function ExecuteTimerFunction(params)
    local instanceId = params.wtOwner:GetInstanceId()
    if timerDesc[instanceId] ~= nil then
		if timerDesc[instanceId].enabled == 0 then return end
        local func = timerDesc[instanceId].func
        func(timerDesc[instanceId].args == nil and nil or timerDesc[instanceId].args)
		if timerDesc[instanceId].enabled == 2 then return end
		timerDesc[instanceId].widget:PlayFadeEffect( 1.0, 1.0, timerDesc[instanceId].time, EA_MONOTONOUS_INCREASE )
    else
		LogInfo("NOT FOUND TIMER")
	end
end

function ExecuteTimerFunctionOnce(params)
    local instanceId = params.wtOwner:GetInstanceId()
    if timerDesc[instanceId] ~= nil then
        local func = timerDesc[instanceId].func
        func(timerDesc[instanceId].args == nil and nil or timerDesc[instanceId].args)
		DestroyTimer(instanceId)
    end
end

function StartTimer(instanceId, isOnce)
	local wt = timerDesc[instanceId]
	if wt ~= nil then
		wt.enabled = isOnce == true and 2 or 1
		wt.widget:PlayFadeEffect( 1.0, 1.0, wt.time, EA_MONOTONOUS_INCREASE )
	end
end

function StopTimer(instanceId)
	local wt = timerDesc[instanceId]
	if wt ~= nil then
		wt.enabled = 0
		wt.widget:FinishFadeEffect()
	end
end

function DestroyTimer(instanceId)
	local wt = timerDesc[instanceId]
	if wt ~= nil then
		wt.widget:FinishFadeEffect()
		wt.widget:DestroyWidget()
		timerDesc[instanceId] = nil
		return true
	end
	return false
end

function InitTimer(func, time, isOnce, args)
    local wtTimerWidget = mainForm:CreateWidgetByDesc( timerWidgetDesc )
	local instanceId = wtTimerWidget:GetInstanceId()
    wtTimerWidget:Show(false)
	wtTimerWidget:SetName("")
	timerDesc[instanceId] = {}
	timerDesc[instanceId].func = func
	timerDesc[instanceId].time = time
	timerDesc[instanceId].widget = wtTimerWidget
    timerDesc[instanceId].args = args or nil
    timerDesc[instanceId].enabled = 0
	common.RegisterEventHandler( isOnce == true and ExecuteTimerFunctionOnce or ExecuteTimerFunction , "EVENT_EFFECT_FINISHED", {wtOwner = wtTimerWidget})
	if isOnce then
		wtTimerWidget:PlayFadeEffect( 1.0, 1.0, time, EA_MONOTONOUS_INCREASE )
	end		
	return instanceId
end

InitTimer0()
