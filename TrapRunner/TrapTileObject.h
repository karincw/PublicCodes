#pragma once
#include "Object.h"
class TrapTileObject :
    public Object
{
public:
	bool isEnter = false;
public:
	TrapTileObject();
	~TrapTileObject();
public:
	void Update() override;
	void Render(HDC _hdc) override;
public:
	void ExplosionTile(Object* owner);
	void EnterCollision(Collider* _other)override;
	void StayCollision(Collider* _other)override;
	void ExitCollision(Collider* _other)override;
};

