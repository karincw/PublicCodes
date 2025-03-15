#pragma once
class Collider;
class Component;
class Transform;
class Object
{
public:
	Object();
	virtual ~Object();
public:
	virtual void Update() abstract;
	virtual void LateUpdate();
	virtual void Render(HDC _hdc) abstract;
	void ComponentRender(HDC _hdc);
public:
	virtual void EnterCollision(Collider* _other);
	virtual void StayCollision(Collider* _other);
	virtual void ExitCollision(Collider* _other);
	const bool& GetIsDead() const { return m_IsDie; }
	const bool& GetDieToDelete() const { return m_dieToDelete; }
	void SetDead(bool s = false) { m_IsDie = true; m_dieToDelete = s; }
	void SetName(wstring _name) { m_name = _name; }
	const wstring& GetName() const { return m_name; }

private:
	bool m_dieToDelete = false;
	bool m_IsDie = false;
	wstring m_name;
public:
	template<typename T>
	void AddComponent()
	{
		T* com = new T;
		com->SetOwner(this);
		m_vecComponents.push_back(com);
	}
	template<typename T>
	void AddComponent(T* compo)
	{
		compo->SetOwner(this);
		m_vecComponents.push_back(compo);
	}
	template<typename T>
	T* GetComponent()
	{
		T* component = nullptr;
		for (Component* com : m_vecComponents)
		{
			component = dynamic_cast<T*>(com);
			if (component)
				break;
		}
		return component;
	}
	template<typename T>
	std::vector< T*> GetComponents()
	{
		std::vector< T*> components = {};
		for (Component* com : m_vecComponents)
		{
			T* compo = dynamic_cast<T*>(com);
			if (compo)
				components.push_back(compo);
		}
		return components;
	}
	Transform* GetTransform()
	{
		return dynamic_cast<Transform*>(m_vecComponents[0]);
	}
protected:
	vector<Component*> m_vecComponents;
};

