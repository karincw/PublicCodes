#pragma once
#include "Collider.h"
class CircleCollider :
	public Collider
{
public:
	CircleCollider();
	~CircleCollider();
public:
	void LateUpdate() override;
	void Render(HDC _hdc) override;
public:
	float GetRadius() { return _radius; };
	void SetRadius(float rad)
	{
		_radius = rad;
		m_vSize = { rad * 2,rad * 2 };
	};
	void SetSize(Vec2 _vSize) override
	{
		auto m = min(_vSize.x, _vSize.y);
		_radius = m / 2;
		m_vSize = { m,m };
	}

private:
	float _radius = 0;
};

