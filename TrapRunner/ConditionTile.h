#pragma once
#include "TileObject.h"
#include "Conditionable.h"
class Condition;
class ConditionTile :
	public TileObject, public Conditionable
{
public:
	ConditionTile();
	~ConditionTile();
public:
	void Update() override;
	void Render(HDC _hdc) override;
	void ApplyCondition();

};

