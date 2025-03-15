#pragma once
#include "pch.h"

class DelayRun
{
public:
	DelayRun(float time, std::function<void(void)> func);
	~DelayRun();
public:


};