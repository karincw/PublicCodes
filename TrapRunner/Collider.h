#pragma once
#include "Component.h"

template<typename t, typename... Types>
class VariadicAction;
class Object;

class Collider : public Component
{
public:
	Collider();
	virtual ~Collider();
public:
	// Component을(를) 통해 상속됨
	void LateUpdate() override;
	void Render(HDC _hdc) override;
public:
	void EnterCollision(Collider* _other); // 충돌진입
	void StayCollision(Collider* _other); // 충돌중
	void ExitCollision(Collider* _other); // 충돌해제
	const UINT& GetID() const { return m_ID; }
public:
	virtual void SetSize(Vec2 _vSize) { m_vSize = _vSize; }
	const Vec2& GetSize() const { return m_vSize; }
	void SetOffSetPos(Vec2 _vOffsetPos)
	{
		m_vOffsetPos = _vOffsetPos;
	}
	const Vec2& GetOffSetPos() const { return m_vOffsetPos; }
	const Vec2& GetLatedUpatedPos() const
	{
		return m_vLatePos;
	}
	void UpdateLatedUpatedPos();
public:
	std::unique_ptr<VariadicAction<Collider*, Object*>> collisionEnterEvent;
	std::unique_ptr<VariadicAction<Collider*, Object*>> collisionStayEvent;
	std::unique_ptr<VariadicAction<Collider*, Object*>> collisionExitEvent;
protected:
	bool m_showDebug = false;
	UINT m_ID; // 충돌체 고유 ID값
	static UINT m_sNextID;

	Vec2 m_vSize; // 콜라이더 크기
	// Object로부터 상대적인 위치
	Vec2 m_vOffsetPos;
	// LateUpdate에서 매 프레임마다 오브젝트로부터 계산되는 위치
	Vec2 m_vLatePos;

};

