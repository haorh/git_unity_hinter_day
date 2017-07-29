namespace EasyNS
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq.Expressions;
    using System;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    static class Easy
    {
        //use this if only 1 clip 
        static public bool StartAnimation(ref Animator anim, string animationClip, float speed)
        {
            var animationState = anim.GetCurrentAnimatorStateInfo(0);

            if (animationState.shortNameHash == Animator.StringToHash(animationClip))//if the name is same, check speed
            {
                if (animationState.normalizedTime >= 1)//end of animation
                    return true;
                if (anim.GetFloat("Speed") == speed)// || anim.GetFloat("Speed") == 0)//if its already in motion, break
                    return false;
            }
            float animationTime = animationState.normalizedTime;

            if (animationTime > 0 && animationTime < 1)
                anim.Play(animationClip, 0, animationTime);//play this if menu is in transition
            else
                anim.Play(animationClip, 0, 0);

            anim.SetFloat("Speed", speed);//if your anim object is false, error occur        
            return false;
        }

        //use this if involve auto playing to nextclip without condition
        static public bool StartAnimation(ref Animator anim, string animationClip, float speed, string nextAnimClip, float speedOfNextClip)
        {
            //if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.ToString()==animationClip)
            var animationState = anim.GetCurrentAnimatorStateInfo(0);

            //if (anim.ToString() == "StandHereImg")
            //    Debug.Log("Startanim = " + anim.ToString() + ", animationClip " + animationClip + ", spd = " + anim.GetFloat("Speed") + ", time = " + animationState.normalizedTime);

            if (animationState.shortNameHash == Animator.StringToHash(animationClip))//if the name is same, check speed
            {
                if (animationState.normalizedTime >= 1)//end of animation
                    return true;

                if (anim.GetFloat("Speed") == speed)// || anim.GetFloat("Speed") == 0)//if its already in motion, break
                    return false;
            }
            else if (nextAnimClip != null)//assuming that the next animation is autoplayed in animator, enter here
            {
                if (animationState.shortNameHash == Animator.StringToHash(nextAnimClip))//enter here to prevent 1st animation to be replayed forever
                {
                    anim.SetFloat("Speed", speedOfNextClip);
                    return true;
                }
            }
            //Debug.Log("Continue here "+ animationState.normalizedTime);
            //Debug.Log("anim = " + anim.ToString()+ ", animationClip "+ animationClip);
            //if (anim.ToString() == "StandHereImg")
            //    Debug.Log("StartSuccess, anim = " + anim.ToString());

            float animationTime = animationState.normalizedTime;
            //anim.SetFloat("Speed", 1);
            if (animationTime > 0 && animationTime < 1)
                anim.Play(animationClip, 0, animationTime);//play this if menu is in transition
            else
                anim.Play(animationClip, 0, 0);

            //StartCoroutine(SetAnimationSpeed(anim, "Speed", speed));
            anim.SetFloat("Speed", speed);//if your anim object is false, error occur        

            //StartCoroutine(SetAnimationSpeed(anim, "Speed", speed, conditionName, nextAnimClip));
            return false;
        }

        //this if involve auto playing to nextclip with multiple condition
        static public bool StartAnimation(ref Animator anim, string animationClip, float speed, string nextAnimClip, float speedOfNextClip, string conditionName)
        {
            //if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.ToString()==animationClip)
            var animationState = anim.GetCurrentAnimatorStateInfo(0);
            if (animationState.shortNameHash == Animator.StringToHash(animationClip))//if the name is same, check speed
            {
                if (animationState.normalizedTime >= 1)//end of animation
                    return true;

                if (anim.GetFloat("Speed") == speed)// || anim.GetFloat("Speed") == 0)//if its already in motion, break
                    return false;
            }
            else if (nextAnimClip != null)//assuming that the next animation is autoplayed in animator, enter here
            {
                if (animationState.shortNameHash == Animator.StringToHash(nextAnimClip))//enter here to prevent 1st animation to be replayed forever
                {
                    anim.SetFloat("Speed", speedOfNextClip);
                    return true;
                }
            }

            float animationTime = animationState.normalizedTime;
            //anim.SetFloat("Speed", 1);
            if (animationTime > 0 && animationTime < 1)
                anim.Play(animationClip, 0, animationTime);//play this if menu is in transition
            else
                anim.Play(animationClip, 0, 0);

            //StartCoroutine(SetAnimationSpeed(anim, "Speed", speed, conditionName, nextAnimClip));
            anim.SetFloat("Speed", speed);//if your anim object is false, error occur
            if (conditionName != null)
                anim.SetInteger(conditionName, GetIntConditionFromClipName(nextAnimClip));

            return false;
        }

        static int GetIntConditionFromClipName(string animClip)//hardcode
        {
            int temp = 0;
            if (animClip == "NothingCanvas" || animClip == "NothingUI")
                temp = 0;
            else if (animClip == "BlinkCanvas" || animClip == "BlinkUI")
                temp = 1;
            else if (animClip == "FloatCanvas" || animClip == "FloatUI")
                temp = 2;
            else
                Debug.Log("Invalid animationclip name");

            return temp;
        }

        static public bool EndAnimation(ref Animator anim, string animationClip, float speed)
        {

            if (!anim.isInitialized)//enter here when animation has reached default Animation Clip and its playback time is 0
                return false;
            //if(anim.ToString()=="StandHereImg")
            //if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.ToString()==animationClip)
            var animationState = anim.GetCurrentAnimatorStateInfo(0);
            //Debug.Log("Endanim = " + anim.ToString() + ", animationClip " + animationClip + ", spd = " + anim.GetFloat("Speed") + ", time = "+ animationState.normalizedTime);
            if (animationState.shortNameHash == Animator.StringToHash(animationClip))//if the name is same, check speed
            {
                if (animationState.normalizedTime <= 0)//play once before it is set to initialize
                    return true;

                if (anim.GetFloat("Speed") == speed)//|| anim.GetFloat("Speed")==0)
                    return false;
            }
            //if (anim.ToString() == "StandHereImg")
            //   Debug.Log("EndSuccess, anim = " + anim.ToString());
            float animationTime = animationState.normalizedTime;
            if (animationTime > 0 && animationTime < 1)
                anim.Play(animationClip, 0, animationTime);
            else
                anim.Play(animationClip, 0, 1);
            //StartCoroutine(SetAnimationSpeed(anim, "Speed", speed));
            anim.SetFloat("Speed", speed);//if your anim object is false, error occur        

            return false;
        }
        /// <summary>
        /// Attempt to GetComponent from specfiedGameObject, return error message if failed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">Insert your public viarable that you wish to be initialized</param>
        /// <param name="specfiedGameObject">Insert the GameObject which your variable should reference</param>
        /// <param name="component">Insert 'GetType()' so that the return error message is meaningful (if there is error)</param>
        public static void CheckInspector<T>(ref T variable, GameObject specfiedGameObject, Type component)//try to assign
        {
            if (EqualityComparer<T>.Default.Equals(variable, default(T)))
            {
                if (specfiedGameObject != null)
                {
                    if (typeof(T).Name == "GameObject")
                    {
                        variable = (T)(object)specfiedGameObject;
                    }
                    else
                    {
                        if (!EqualityComparer<T>.Default.Equals(specfiedGameObject.GetComponent<T>(), default(T)))
                            variable = specfiedGameObject.GetComponent<T>();
                        else
                            Debug.LogError("Cannot find " + typeof(T).Name + " component in " + specfiedGameObject.name + " for " + component.Name + " to reference");//look at the displayed error msg
                    }
                }
                else
                    Debug.LogError("Cannot find <specfiedGameObject> for " + component.Name + " to reference");
            }
        }
        /// <summary>
        /// Attempt to GetComponent from specifiedGameObject by using GameObject.Find(), return error message if failed<
        /// </summary>
        /// <typeparam name="T">/typeparam>
        /// <param name="variable">Insert your public viarable that you wish to be initialized</param>
        /// <param name="specifiedGameObject">Insert the GameObject which your variable should reference</param>
        /// <param name="component">Insert 'GetType()' so that the return error message is meaningful (if there is error)</param>
        public static void CheckInspector<T>(ref T variable, string specifiedGameObject, Type component)//try to assign
        {
            if (EqualityComparer<T>.Default.Equals(variable, default(T)))
            {
                var obj = GameObject.Find(specifiedGameObject);
                if (obj != null)
                {
                    if (typeof(T).Name == "GameObject")
                    {
                        variable = (T)(object)obj;
                    }
                    else
                    {
                        if (!EqualityComparer<T>.Default.Equals(obj.GetComponent<T>(), default(T)))
                            variable = obj.GetComponent<T>();
                        else
                            Debug.LogError("Cannot find " + typeof(T).Name + " component in " + specifiedGameObject + " for " + component.Name + " to reference");
                    }
                }
                else
                    Debug.LogError("Cannot find " + specifiedGameObject + " in game");
            }
        }

        public static void CheckInspector<T>(T generic, Type component)//just show msg without assign
        {
            if (EqualityComparer<T>.Default.Equals(generic, default(T)))
            {
                Debug.LogError("Please assign " + typeof(T).Name + " into the inspector for " + component.Name + " to reference");//please read the debug to understand it
                //Debug.LogError("Please assign " + MemberInfoGetting.GetMemberName(() => generic) + " into the inspector");//return name of variable, eg int x, return x
                //Debug.LogError("Please assign " + typeof(T).GetProperties()[0].Name + " into the inspector");
            }
        }

        public static class MemberInfoGetting //nameOf
        {
            public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
            {
                MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
                return expressionBody.Member.Name;
            }
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// This is a function to replace node = node["xxx"]
        /// </summary>
        /// <param name="input">www.text</param>
        /// <param name="node">Example : if the node is ["Deck"], pass "Deck". This string must be unique within the entire www.text</param>
        /// <param name="nodeStartChar">The character at the last index of node + 3, most probably '['</param>
        /// <param name="nodeEndChar">Most probably ']'</param>
        /// <param name="index">Example : if you wan value from {"Deck":[null, "value",null,null], pass 1</param>
        /// <param name="seperator">Example : {"Deck":["value",null,null,null], pass ','. If only one data, pass '@' </param>
        /// <param name="dataStartChar">Example : {"Deck":[{"value"},null,null,null], pass '{'</param>
        /// <param name="dataEndChar">Example : {"Deck":["value",null,null,null], pass '"'</param>
        /// <param name="nullValue">Example : {"Deck":["value",null,null,null], pass "null"</param>
        /// <param name="debugOn">Display debug message</param>
        /// <returns></returns>
        public static string DeckString(string input, string node, char seperator, int index, char dataStartChar, char dataEndChar, string nullValue, bool debugOn)
        {
            if (debugOn)
                Debug.Log("0) Start");
            //possibility
            // 1 - > {"Deck":["{"deckCo.........ngID":4,"unlocked":true}]}",null,null,null,null,null,null,null,null],"Status":true,"Error":""}
            // 2 - > {"Deck":[null,null,null,null,null,null,null,null,null],"Status":true,"Error":""}
            // 3 - > {"Deck":[{"deckCoordinate":"1,1","deckI
            // 4 - > [{"Deck":"","Error":""}]
            // 5 - > "Deck":"{"number":10,"numberArray":[10,20,30,40,50]}"
            // 6 - > "Deck":{"number":10,"numberArray":[10,20,30,40,50]}
            // 7 - > "Deck":null

            //from - > [{"UserID":"592b7e3d4b678094949","Deck":"{"number":10,"numberArray":[10,20,30,40,50]}","Error":""}]
            //to - > {"number":10,"numberArray":[10,20,30,40,50]}

            int nodeFirstIndex = input.IndexOf(node);//don use this as it is confusing
            int columnIndex = nodeFirstIndex + node.Length + 1;

            char nodeStartChar = input[columnIndex + 1];
            char nodeEndChar = '\"';

            if (nodeStartChar == '\"')
                nodeEndChar = '\"';
            else if (nodeStartChar == '[')
                nodeEndChar = ']';
            else if (nodeStartChar == '{')
                nodeEndChar = '}';
            else if (nodeStartChar == 'n')
                return "null";
            else
                Debug.LogError("Dunno nodeEndChar because nodeStartChar is " + nodeStartChar);



            // used only in loop
            //int debugCounter = 1;
            bool startOfData = true;
            int numberOfBracket = 0, commaCounter = 0;
            int startReadIndex = columnIndex + 2;//inclusive, +2     
            //Example -> {"Deck":[{"deckCoordinate
            //nodeReadIndex  = D
            //columnIndex    = :
            //nodeStartChar  = [ or " or n
            //startReadIndex = " or [ or { or n          

            //attribute
            if (seperator == '@')
                index = 0;

            if (nodeStartChar == dataStartChar)//case  6
                startReadIndex = columnIndex + 1;

            //result variable
            int endIndex = -1, startIndex = -1;

            for (int x = startReadIndex; x < input.Length; x++)
            {
                if (startOfData)
                {
                    if (String.Compare(input, x, nullValue, 0, nullValue.Length) == 0)
                    {
                        //if the nullValue is "", it will enter here
                        if (nullValue.Length == 0)//entering here is unwanted behaviour, input[x] should be {
                        {
                            startIndex = x;
                            numberOfBracket++;

                            if (input[x + 1] == dataEndChar)//found end
                            {
                                endIndex = x;
                                //numberOfBracket--;
                                if (commaCounter == index)//found right index
                                {
                                    if (debugOn)
                                        Debug.Log("Break there");
                                    break;
                                }
                            }
                            else // its normal value, continue counting as normal
                            {
                                //Debug.LogError("Unpredicted value where dataEndChar in null is " + input[x]);
                            }
                        }
                        else
                        {
                            startIndex = x;
                            endIndex = x + nullValue.Length - 1;

                            if (commaCounter == index)
                            {
                                if (debugOn)
                                    Debug.Log("Break here");
                                break;
                            }
                            else //not correct index, loop again
                            {
                                x = endIndex;
                                //continue;
                            }
                        }
                    }
                    else if (input[x] == dataStartChar)
                    {
                        //Debug.Log("Entered");
                        startIndex = x;
                        numberOfBracket++;
                    }
                    else
                        Debug.LogError("Unpredicted value where startchar is " + input[x]);

                    startOfData = false;
                }
                else
                {
                    if (numberOfBracket == 1)//possible exit
                    {
                        if (input[x] == dataEndChar)//found end
                        {
                            endIndex = x;
                            numberOfBracket--;
                            if (commaCounter == index)//found right index
                            {
                                if (debugOn)
                                    Debug.Log("Break there");
                                break;
                            }
                            //else
                            //    Debug.Log("Found data end but not exit where next is "+ input[x+1]);
                        }
                        else if (input[x] == dataStartChar)// more unwanted data
                        {
                            numberOfBracket++;
                            continue;
                        }
                    }
                    else if (numberOfBracket > 1)
                    {
                        if (input[x] == dataStartChar)// more unwanted data
                        {
                            numberOfBracket++;
                            //continue;
                        }
                        else if (input[x] == dataEndChar) //reduce unwanted data
                        {
                            numberOfBracket--;
                            //continue;
                        }
                    }
                    else if (numberOfBracket == 0)
                    {
                        if (input[x] == nodeEndChar)
                        {
                            if (index > commaCounter)
                            {
                                Debug.LogError("Can't find index = " + index + " as there is only " + (commaCounter + 1) + " item in this list");
                                break;
                            }

                        }
                        else if (input[x] == seperator)
                        {
                            if (debugOn)
                                Debug.Log("Start of Index " + (commaCounter + 1) + ", where the next 5 chars is " + input.Substring(x, 5));
                            commaCounter++;
                            startOfData = true;
                        }
                        else
                            Debug.LogError("Unpredicted value where no more Bracket is " + input[x]);
                    }
                }
                //--end of loop
            }

            int dataLength = endIndex - startIndex + 1;

            if (debugOn)
            {
                if (input[nodeFirstIndex] != node[0])
                    Debug.LogError("1) nodeReadIndex Failed as it should be " + node[0] + "it is " + input[nodeFirstIndex]);

                if (input[columnIndex] != ':')
                    Debug.LogError("1) columnIndex Failed as it should be ':' but it is " + input[columnIndex]);

                Debug.Log("2) numberOfBracket = " + numberOfBracket + ", commaCounter = " + commaCounter + ", startOfData = " + startOfData);
                Debug.Log("3) length = " + dataLength + ", where start = " + startIndex + " and end = " + endIndex + ", input length = " + input.Length);
            }


            var filteredString = input.Substring(startIndex, dataLength);//remove ""

            if (filteredString.Length > 0)
                if (filteredString[0] == '\"')
                    filteredString = input.Substring(startIndex + 1, dataLength - 2);

            if (debugOn)
                Debug.Log("4) Result = " + filteredString);

            return filteredString;
        }

        public static T SafeCall<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                Debug.LogError(" ex.Message = " + ex.Message);
            }

            return default(T);// return default type if an error occured
        }

        public static string TryRemoveBracket(string input)
        {
            if (input[0] == '\"')
            {
                if (input[input.Length - 1] == '\"')
                    return input.Substring(1, input.Length - 2);
            }
            else if (input[0] == '[')
            {
                if (input[input.Length - 1] == ']')
                    return input.Substring(1, input.Length - 2);
            }
            else if (input[0] == '{')
            {
                if (input[input.Length - 1] == '}')
                    return input.Substring(1, input.Length - 2);
            }
            else if (input[0] == '(')
            {
                if (input[input.Length - 1] == ')')
                    return input.Substring(1, input.Length - 2);
            }
            return input;

        }

        public static bool CheckIfExisted(List<List<int>> nestedList, List<int> newList)
        {
            foreach (List<int> existedRegion in nestedList)
                if (CompareList(existedRegion, newList))
                    return true;
            return false;
        }

        public static bool CompareList<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]++;
                else
                    cnt.Add(s, 1);
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]--;
                else
                    return false;
            }
            return cnt.Values.All(c => c == 0);
        }

        public static void DebugList<T>(List<T> list)
        {
            SimpleJSON.JSONArray test = new SimpleJSON.JSONArray();
            foreach (T t in list)
                test.Add(t.ToString());
            Debug.Log("List = " + test.ToString());
        }

        public static T CopyComponent<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static T AddComponent<T>(GameObject go, T toAdd) where T : Component
        {
            return CopyComponent(go.AddComponent(toAdd.GetType()), toAdd);

            //return go.AddComponent<T>().CopyComponent(toAdd) as T;//doesn't work
        }
    }
}
