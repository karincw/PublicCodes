#pragma once
#include "pch.h"

template<typename t>
class Action
{
public:
	Action()
		:funcs({})
	{

	}
	~Action()
	{

	}

public:

	using FuncvoidType = void(*)(t);
	// 반환형(*=> 포인터이다)(인자)
	//// 함수포인터
	//typedef;
	//using;  

	void Invoke(t value)
	{
		if (funcs.size() <= 0) return;
		for (auto f : funcs)
		{
			f(value);
		}
	}

	void Insert(FuncvoidType f)
	{
		//funcs.push_back(f);
		funcs.emplace_back(f);
	}
	 
	std::vector<FuncvoidType> funcs;
};

template<typename T, typename... Types>
class VariadicAction
{
public:
	VariadicAction()
		:funcs({})
	{

	}
	~VariadicAction()
	{

	}

public:

	using FuncvoidType = void(*)(T, Types...);
	// 반환형(*=> 포인터이다)(인자)
	//// 함수포인터
	//typedef;
	//using;  

	void Invoke(T t, Types... value)
	{
		if (funcs.empty()) return;
		for (auto f : funcs)
		{
			f(t, value...);
		}
	}

	void Insert(FuncvoidType f)
	{
		//funcs.push_back(f);
		funcs.emplace_back(f);
	}

	std::vector<FuncvoidType> funcs;
};
