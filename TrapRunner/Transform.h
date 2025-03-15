#pragma once
#include "Component.h"

class Collider;
class Transform : public Component
{
public:
	Transform();
	~Transform();
public:
	void LateUpdate() override;
	void Render(HDC _hdc) override;
public:
	void Translate(Vec2 v);
public:
	void SetPosition(Vec2 v);
	void SetScale(Vec2 v) { _scale = v; }
	Vec2 GetPosition() { return _position; };
	Vec2 GetScale() { return _scale; };
private:
	Vec2 _position;
	Vec2 _scale;
};

