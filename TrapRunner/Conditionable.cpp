#include "pch.h"
#include "Condition.h"
#include "Conditionable.h"

Conditionable::Conditionable()
	: conditions{}
{
}

Conditionable::~Conditionable()
{
}


bool Conditionable::CheckCondiiton()
{
	bool state = true;
	for (auto c : conditions)
	{
		if (c->condition == false)
			state = false;
	}
	return state;
}

void Conditionable::AddCondiiton(Condition* c)
{
	conditions.push_back(c);
}