using System;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace MeshDeformation.JobSystemDeformer
{
    public class JobSystemDeformer : BaseDeformer
    {
        public Button _button;

        private NativeArray<Vector3> _vertices;
        private bool _scheduled;
        private DeformerJob _job;
        private JobHandle _handle;
        private TMP_Text _btnText;

        protected override void Awake()
        {
            base.Awake();
            _vertices = new NativeArray<Vector3>(Mesh.vertices, Allocator.Persistent);
            _button.onClick.AddListener(OnClick);
            _btnText = _button.GetComponentInChildren<TMP_Text>();
            UpdateButtonText();
        }
        
        private void Update()
        {
            TryScheduleJob();
        }

        private void LateUpdate()
        {
            CompleteJob();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
            _vertices.Dispose();
        }

        private void OnClick()
        {
            var nextValue = JobSystemModel.BatchSize * 2;
            if (nextValue > Math.Pow(2, 10))
            {
                nextValue = 1;
            }

            JobSystemModel.BatchSize = nextValue;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            _btnText.text = $"Batch size: {JobSystemModel.BatchSize}";
        }

        private void TryScheduleJob()
        {
            if (_scheduled)
            {
                return;
            }

            _scheduled = true;
            _job = new DeformerJob(_speed, _amplitude, Time.time, _vertices);
            _handle = _job.Schedule(_vertices.Length, JobSystemModel.BatchSize);
        }

        private void CompleteJob()
        {
            if (!_scheduled)
            {
                return;
            }

            _handle.Complete();
            Mesh.MarkDynamic();
            Mesh.SetVertices(_vertices);
            // Mesh.RecalculateNormals();
            _scheduled = false;
        }
    }
}