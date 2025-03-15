#pragma once
#include "Object.h"
class Direction :
	public Object
{
public:
	Direction(wstring dir);
	~Direction();
	void Update() override;
	void Render(HDC _hdc) override;
};

