using PrimeTween;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private Donut _selectedDonut;
    private Post _selectedPost;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform != null && hit.transform.TryGetComponent(out Post post))
                {
                    if (_selectedDonut == null) 
                    {
                        if (post.HaveDonuts())
                        {
                            _selectedDonut = post.PopDonut();
                            _selectedPost = post;
                            if (_selectedDonut != null)
                            {
                                AnimateDonutSelection(post, _selectedDonut);
                            }
                        }
                    }
                    else
                    {
                        if (post.CanAddDonut(_selectedDonut))
                        {

                            if(post == _selectedPost)
                            {
                                AnimateSelectedDonutReturnInPost(post);
                                post.AddDonut(_selectedDonut);
                            }
                            else
                            {
                                AnimateDonutMoveToPost(post, _selectedDonut);
                                post.AddDonut(_selectedDonut);
                                _selectedPost.OpenTopDonutColor();                                                                                              
                            }

                            _selectedDonut = null;
                            _selectedPost = null;
                        }
                        else
                        {
                            AnimateSelectedDonutShake();
                        }
                    }
                }
                
            }
        }
    }

    private Sequence AnimateDonutSelection(Post post, Donut donut)
    {        
        Tween.CompleteAll();
        Vector3 donutPos = donut.transform.position;
        return Sequence.Create()
            .Group(Tween.PositionY(donut.transform, endValue: post.transform.position.y + 5, duration: .5f))
            .Group(Tween.PunchScale(donut.transform, new Vector3(.5f, 0, .5f), duration: .5f, frequency: 5));            
    }

    private void AnimateSelectedDonutReturnInPost(Post post)
    {
        Tween.PositionY(_selectedDonut.transform, endValue: post.GetDonutYPosition(), duration: .7f);
        Tween.PunchScale(_selectedDonut.transform, new Vector3(.5f, 0, .5f), duration: .7f, frequency: 5);
    }

    private Sequence AnimateDonutMoveToPost(Post post, Donut donut)
    {
        Tween.CompleteAll();

        Vector3 postPos = post.transform.position;
        Vector3 newDonutPos = new Vector3(postPos.x, postPos.y + 5, postPos.z);
        return Sequence.Create()
        .Group(Tween.Position(donut.transform, endValue: newDonutPos, duration: .5f))
        .Chain(Tween.PositionY(donut.transform, endValue: post.GetDonutYPosition(), duration: .5f)
        .Group(Tween.PunchScale(donut.transform, new Vector3(.5f, 0, .5f), duration: .5f, frequency: 5)));        
    }

    private void AnimateSelectedDonutShake() => Tween.ShakeLocalPosition(_selectedDonut.transform, Vector3.one / 2, .5f);
}

