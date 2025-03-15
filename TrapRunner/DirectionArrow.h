#pragma once
#include "Object.h"

enum class SELECT_TYPE
{
	PLAY = 0,
	EXIT = 1,
};

class DirectionArrow :
    public Object
{
public:
	DirectionArrow();
	~DirectionArrow();
public:
	void Update() override;
	void Render(HDC _hdc) override;
public:
	void EnterCollision(Collider* _other)override;
	void StayCollision(Collider* _other)override;
	void ExitCollision(Collider* _other)override;
public:
	void ChangeTypeToExit();
	void ChangeTypeToPlay();
private:
	SELECT_TYPE selectType = SELECT_TYPE::PLAY;
	int playPos = 300;
	int exitPos = 400;
};

