#include "pch.h"
#include "Animation.h"
#include "Animator.h"
#include "Object.h"
#include "Texture.h"
#include "TimeManager.h"
#include "Action.h"
#include "Camera.h"
#include "Scene.h"
#include "SceneManager.h"
Animation::Animation()
	: m_pAnimator(nullptr)
	, m_CurFrame(0)
	, m_pTex(nullptr)
	, m_fAccTime(0.f)
	, m_reSizeRatioX(0.f)
	, m_reSizeRatioY(0.f)
	, animationEndEvent(nullptr)
	, eventflag(false)
{
	animationEndEvent = new Action<Object*>();
}

Animation::~Animation()
{
	delete animationEndEvent;
}

void Animation::Update()
{
	if (m_pAnimator->GetRepeatcnt() <= 0)
	{
		if (eventflag)
		{
			Object* pObj = m_pAnimator->GetOwner();
			animationEndEvent->Invoke(pObj);
			eventflag = false;
		}
		//여기서 애니메이션이 끝났을때 액션 발행
		m_CurFrame = m_vecAnimFrame.size() - 1;
		return;
	}
	m_fAccTime += fDT;
	// 누적한 시간이 내가 이 프레임에 진행한 시간을 넘어섰냐?
	if (m_fAccTime >= m_vecAnimFrame[m_CurFrame].fDuration)
	{
		// 일단 모은 시간에서 현재 진행한 시간을 빼고
		m_fAccTime -= m_vecAnimFrame[m_CurFrame].fDuration;
		++m_CurFrame; // 다음프레임으로 옮기기

		if (m_CurFrame >= m_vecAnimFrame.size()) // 한바퀴 돌게하고싶어
		{
			if (!m_pAnimator->GetRepeat())
				m_pAnimator->SetRepeatcnt();
			m_CurFrame = 0;
			m_fAccTime = 0.f;
		}
	}
}

void Animation::Render(HDC _hdc)
{
	Object* pObj = m_pAnimator->GetOwner();
	Transform* trm = pObj->GetComponent<Transform>();
	Vec2 position = trm->GetPosition();
	Vec2 scale = trm->GetScale();

	// 오프셋 
	position = position + m_vecAnimFrame[m_CurFrame].vOffset;

	Camera* cam = GET_SINGLE(SceneManager)->GetCurrentScene()->GetCamera();
	position -= cam->GetTransform()->GetPosition();

	TransparentBlt(_hdc
		, (int)(position.x - scale.x / 2)
		, (int)(position.y - scale.y / 2)
		, scale.x
		, scale.y
		, m_pTex->GetTexDC()
		, (int)(m_vecAnimFrame[m_CurFrame].vLT.x * m_reSizeRatioX)
		, (int)(m_vecAnimFrame[m_CurFrame].vLT.y * m_reSizeRatioY)
		, (int)(m_vecAnimFrame[m_CurFrame].vSlice.x * m_reSizeRatioX)
		, (int)(m_vecAnimFrame[m_CurFrame].vSlice.y * m_reSizeRatioY)
		, RGB(255, 0, 255));
}

void Animation::Create(Texture* _pTex, Vec2 _vLT, Vec2 _vSliceSize, Vec2 _vStep, int _framecount, float _fDuration, bool _isRotate)
{
	Object* pObj = m_pAnimator->GetOwner();
	Transform* trm = pObj->GetComponent<Transform>();
	Vec2 position = trm->GetPosition();
	Vec2 scale = trm->GetScale();
	m_reSizeRatioX = scale.x / _vSliceSize.x;
	m_reSizeRatioY = scale.y / _vSliceSize.y;
	m_pTex = _pTex;

	// 리사이즈 이미지
	m_pTex = m_pTex->MakeStretchTex(m_reSizeRatioX, m_reSizeRatioY);

	m_IsRotate = _isRotate;
	for (int i = 0; i < _framecount; ++i)
	{
		m_vecAnimFrame.push_back(tAnimFrame({ _vLT + _vStep * i,
			_vSliceSize, _fDuration,{0.f,0.f} }));
	}
}
