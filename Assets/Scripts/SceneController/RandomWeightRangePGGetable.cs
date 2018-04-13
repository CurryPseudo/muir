using System;
using System.Collections.Generic;
using UnityEngine;
using SceneController;
using Sirenix.OdinInspector;
namespace SceneController {
    [CreateAssetMenu]
    public class RandomWeightRangePGGetable : NextPutableGoGetableScriptable
    {
        [OnValueChanged("sortList")]
        public List<PutableGoWithWeight> list = new List<PutableGoWithWeight>();
        private void sortList() {
            list.Sort();
        }
        public float min;
        public float max;
        public void AddPutableGo(PutableGoWithWeight pgww) {
            list.Add(pgww);
            list.Sort();
        }
        public override PutableGo GetNextPutableGo()
        {
            if(list.Count == 0) {
                throw new Exception("RandomList should not be empty");
            }
            int minI = findForwardestMin(min);
            int maxI = findBackwardestMax(max);
            if(minI != -1 &&  maxI != -1) {
                return list[UnityEngine.Random.Range(minI, maxI + 1)].putableGo;
            }
            else {
                throw new Exception("Cant Get any valid PutableGo");
            }

        }
        public int findForwardestMin(float min) {
            int minI = 0;
            int maxI = list.Count - 1;
            if(list[minI].weight >= min) {
                return minI;
            }
            if(list[maxI].weight < min) {
                return -1;
            }
            while(maxI - minI >= 2) {
                int midI = (maxI + minI) / 2;
                if(list[midI].weight >= min) {
                    maxI = midI;
                }
                else {
                    minI = midI;
                }
            }
            return maxI;
        }
        public int findBackwardestMax(float max) {
            int minI = 0;
            int maxI = list.Count - 1;
            if(list[maxI].weight <= max) {
                return maxI;
            }
            if(list[minI].weight > max) {
                return -1;
            }
            while(maxI - minI >= 2) {
                int midI = (maxI + minI) / 2;
                if(list[midI].weight <= max) {
                    minI = midI;
                }
                else {
                    maxI = midI;
                }
            }
            return minI;
        }
    }
}