using UnityEngine;
using System.Collections.Generic;
//using ModestTree;
using System.Collections;
using System.Linq;
namespace pathfinding
{

    public class CharacterMovement : MonoBehaviour
    {
        //[Inject]
        private Pathfinder _pathfinder;
       // [Inject]
        private Animator animator;
        //  [Inject]

        [SerializeField]
        private ParticleSystem _system;

        private Transform _target;
        [SerializeField]
        private float _speed = 5f;
        private List<Node> _path;
        private int _targetIndex;
        private Coroutine runningCoroutine;

       public void Start()
        {
            _pathfinder = GetComponent<Pathfinder>();
            animator = GetComponent<Animator>();
            _pathfinder.TargetChanged += SetNewTarget;
        }

        void Update()
        {
            if(_target == null) return;
            if (_path == null)
            {
                _pathfinder.FindPath(transform.position, _target.position);
                _path = _pathfinder.Path;
                _targetIndex = 0;
            }
          
        }

        public void OnDestroy()
        {
            _pathfinder.TargetChanged -= SetNewTarget;
        }

        public void SetNewTarget()
        {
            if (_path != null)
            {
                _pathfinder?.Grid?.HighlightTiles(_path, false);
            }
            _target = _pathfinder.Target;
            _pathfinder.FindPath(transform.position, _target.position);
            _path = _pathfinder.Path;
            _targetIndex = 0;
            if (_path != null && !_path.Any()) return;
            FollowPath();
        }

        public void ChangeSpeed(float newSpeedValue)
        {
            _speed = newSpeedValue;
        }

        void FollowPath()
        {
            if (_path != null)
            {
                if(runningCoroutine != null)
                {
                    StopCoroutine(runningCoroutine);
                }
                runningCoroutine = StartCoroutine(FollowPathCo());
            }
         
        }

        private IEnumerator FollowPathCo()
        {
            _pathfinder.Grid.HighlightTiles(_path,true);
            animator.SetBool("Run", true);
            while (_targetIndex < _path.Count)
            {
                yield return null;
                if(_targetIndex < _path.Count)
                {

                    Node node = _path[_targetIndex];
                    if (node == null)
                        continue;
                    Vector3 targetPosition = new Vector3(node.worldPosition.x, transform.position.y, node.worldPosition.z);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

                    RotateTowards(targetPosition);
                    if (transform.position == targetPosition)
                    {
                        _targetIndex++;
                    }
                }

            }
            animator.SetBool("Run", false);
            _system.gameObject.SetActive(true);
            _system.Play();
            yield return new WaitForSeconds(1);
            _system.Stop();
            _pathfinder.Grid.HighlightTiles(_path, false);
        }

        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _speed);
        }
       // public class Factory : PlaceholderFactory<CharacterMovement> { };
    }
}

