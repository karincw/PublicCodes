#pragma once

class Condition;
class Conditionable
{
public:
	Conditionable();
	virtual ~Conditionable();
public:
	bool CheckCondiiton();
	void AddCondiiton(Condition* c);
protected:
	bool flag = false;
private:
	vector<Condition*> conditions;
};

