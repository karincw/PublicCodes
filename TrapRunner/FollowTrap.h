#pragma once
#include "Object.h"

#define maxSpeed 450
#define AccelationSpeed 1.2
#define DecelationSpeed -0.3f
#define MaxAccelableDir 0.035f

class FollowTrap : public Object
{
public:
	FollowTrap();
	~FollowTrap();
public:
	void Update() override;
	void Render(HDC _hdc) override;
public:
	void Accel();
	void Accelation();
	void Deceleration();
	Object* currentTarget;
private:
	float speed;
	Vec2 moveDir;
};

