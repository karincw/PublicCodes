#include "pch.h"
#include "CircleCollider.h"
#include "GDISelector.h"
#include "SceneManager.h"
#include "Scene.h"
#include "Camera.h"

CircleCollider::CircleCollider()
{
}

CircleCollider::~CircleCollider()
{
}

void CircleCollider::LateUpdate()
{
	const Object* pOwner = GetOwner();
	Transform* trm = m_pOwner->GetComponent<Transform>();
	Vec2 vPos = trm->GetPosition();
	m_vLatePos = vPos + m_vOffsetPos;
}

void CircleCollider::Render(HDC _hdc)
{
	return;
	PEN_TYPE ePen = PEN_TYPE::GREEN;
	if (m_showDebug)
		ePen = PEN_TYPE::RED;
	GDISelector pen(_hdc, ePen);
	GDISelector brush(_hdc, BRUSH_TYPE::HOLLOW);
	Vec2 RenderPos = m_vLatePos - GET_SINGLE(SceneManager)->GetCurrentScene()->GetCamera()->GetTransform()->GetPosition();
	ELLIPSE_RENDER(_hdc, RenderPos.x, RenderPos.y, m_vSize.x, m_vSize.y);
}
