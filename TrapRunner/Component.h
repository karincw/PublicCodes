#pragma once
class Object;
class Component
{
public:
	Component();
	virtual ~Component();
public:
	virtual void LateUpdate() abstract;
	virtual void Render(HDC _hdc) abstract;
public:
	void SetOwner(Object* _owner) { m_pOwner = _owner; }
	Object* GetOwner() const { return m_pOwner; }
public:
	inline void SetEnable(bool state) { _enable = state; }
	inline bool GetEnable() { return  _enable; }
protected:
	Object* m_pOwner;
	bool _enable = true;
};

