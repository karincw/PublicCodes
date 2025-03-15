#pragma once
#include "Object.h"
class Projectile : public Object
{
public:
	Projectile();
	~Projectile();
	void Update() override;
	void Render(HDC _hdc) override;
public:
	void SetAngle(float _f)
	{
		m_angle = _f;
	}
	void SetDir(Vec2 _dir)
	{
		m_vDir = _dir;
		m_vDir.Normalize();
	}
	void SetSpeed(float s)
	{
		m_speed = s;
	}
protected:
	//float m_dir;
	float m_angle;
	float m_speed;
	Vec2 m_vDir;
};

