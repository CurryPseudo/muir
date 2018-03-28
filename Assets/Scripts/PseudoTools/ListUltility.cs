using System.Collections.Generic;
using System;
namespace PseudoTools {
    public class ListUltility {
        public static void DeleteIf<T>(List<T> list, Func<T,bool> removeCondition) {
            List<T> deleteList = new List<T>();
            foreach(var t in list) {
                if(removeCondition(t)) {
                    deleteList.Add(t);
                }
            }
            foreach(var t in deleteList) {
                list.Remove(t);
            }
        }
        public static void DoIf<T>(List<T> list, Func<T,bool> doCondition, Action<T> Do) {
            foreach(var t in list) {
                if(doCondition(t)) {
                    Do(t);
                }
            }
        }
    }
}