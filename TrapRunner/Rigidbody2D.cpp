#include "pch.h"
#include "Rigidbody2D.h"
#include "Object.h"
#include "TimeManager.h"


Rigidbody2D::Rigidbody2D()
	: _gravity(9.8f)
	, _gravityScale(1)
	, _speed({ 0,0 })
{

}

Rigidbody2D::~Rigidbody2D()
{

}

void Rigidbody2D::Addforce(Vec2 dir)
{
	_speed += dir;
}

void Rigidbody2D::SetVelocity(Vec2 velo)
{
	_speed = velo;
}

void Rigidbody2D::LateUpdate()
{
	float dt = GET_SINGLE(TimeManager)->GetDT();
	Transform* trm = m_pOwner->GetComponent<Transform>();
	Vec2 vPos = trm->GetPosition();
	_speed.y += _gravity * _gravityScale * dt;
	vPos += _speed * dt;
	trm->SetPosition(vPos);
}

void Rigidbody2D::Render(HDC _hdc)
{

}
