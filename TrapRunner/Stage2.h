#pragma once
#include "Scene.h"

enum class OBJECT_TYPE
{
	TILE,
	TRAP,
	FALL,
	BUTTON,
	CONDITIONTILE,
	TOWER,
	LAZER,
	FOLLOW,
	GOLD
};

class Stage2 : public Scene
{
	void CreateArrow(Vec2 v, wstring d);
	void Init() override;
	void Render(HDC _hdc) override;
	void Release() override;
	Object* CreateObject(Vec2 vec, OBJECT_TYPE type);
};

