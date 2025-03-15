#pragma once
#include "Object.h"
template <typename T>
class Action;
class Camera;
class RollingSkillUI;

class Agent :
	public Object
{
public:
	Agent();
	~Agent();
public:
	void Update() override;
	void Render(HDC _hdc) override;
	void EnterCollision(Collider* _other)override;
	void StayCollision(Collider* _other)override;
public:
	void PlayAnimation(wstring animationName, bool repeat);
	void Hit();
public:
	bool isRolling;
	bool canRolling;
	bool isHit;
	bool canHit;
	bool isRun;
	bool isGroundCheck = true;

	Object* backUpTile;
	Camera* cam;
	RollingSkillUI* rollingSkillUI;
private:
	bool isRight;
	Vec2 rollingDir;
};
