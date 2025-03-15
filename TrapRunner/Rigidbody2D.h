#pragma once
#include "Component.h"
class Rigidbody2D : public Component
{
public:
	Rigidbody2D();
	~Rigidbody2D();
public:
	void LateUpdate() override;
	void Render(HDC _hdc) override;
public:
	inline void SetGravityScale(float gravityScale)
	{
		_gravityScale = gravityScale;
	}
public:
	void Addforce(Vec2 dir);
	void SetVelocity(Vec2 velo);
private:
	float _gravity;
	float _gravityScale;
	Vec2 _speed;
};

