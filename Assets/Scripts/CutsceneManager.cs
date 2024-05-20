using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class CutsceneManager : MonoBehaviour
{
    [HideInInspector] public static CutsceneManager Instance;
    Coroutine currentCutscene;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A" + this.GetType() + "MonoBehaviour already exists");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StopCutscne()
    {
        if (currentCutscene != null)
        {
            StopCoroutine(currentCutscene);
            currentCutscene = null;
        }
    }

    public void StartCutscne(string csKey)
    {
        Debug.Log("Active? " + gameObject.activeInHierarchy);
        StopCutscne();
        currentCutscene = StartCoroutine(csKey);
    }

    //CUTSCENES
    IEnumerator cafeDiscussion()
    {
        print("trying to start cs");
        GameManager.Instance.Player.GetComponent<Player>().allowControl = false;

        Dialouge[] dialouges = new Dialouge[]
        {
            new Dialouge
            {
                text = "You took your sweet time, huh?",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "Shut up, I just got out of bed. Wait, why is it so dark out?",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "I think I got a lead on that very alive friend of yours - specifically, where he is.",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "Cool - wait, what!? Where is Mako? ",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "I tailed him to an old casino downtown. Last I saw of him, he was sneaking in through the back.",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "Any idea why he'd be going to that kind of place at this hour? ",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Not a clue. He's trash at covering his tracks, but not his motives. You'd have to go and ask him yourself, but that seems like a bad idea.",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "Well now I have to do that.",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Go hop in the car while I make you a coffee, then. I already started it. ",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "I don't need coffee, I'm already wide awake.",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Adrenaline will only get you so far. Now go get in the car.",
                portraitImg = "Nora",
                name = "Nora"
            },
            new Dialouge
            {
                text = "Yeah, yeah, you don't need to tell me twice!",
                portraitImg = "Mari",
                name = "Mari"
            },
            new Dialouge
            {
                text = "You best not drive off on me!",
                portraitImg = "Nora",
                name = "Nora"
            },
        };
        GUIManager.Instance.textbox.CreateTextSequence(dialouges);
        yield return new WaitForEndOfFrame();
        while (GUIManager.Instance.textbox.textArrays == dialouges)
        {
            yield return new WaitForEndOfFrame();
        }
        GameManager.Instance.TransitionToScene("level2");
        GameManager.Instance.Player.GetComponent<Player>().allowControl = true;
    }

    IEnumerator introScene()
    {
        print("trying to start cs");
        GameManager.Instance.Player.GetComponent<Player>().allowControl = false;

        Dialouge[] dialouges = new Dialouge[]
        {
            new Dialouge
            {
                text = "...Good evening, everyone. Now that we're all settled, I'd like to begin.",
                name = "Officiant"
            },
            new Dialouge
            {
                text = "We are gathered here today to honor the memory...",
                name = "Officiant"
            },
            new Dialouge
            {
                text = "How long has it been? Three years now? Yeah, that sounds about right.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "It's been three years since my best friend Mako disappeared - he just vanished without a trace.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "No explanation, no paper trail, nothing. And everyone's done jack about it.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Well, that isn't entirely true. His parents got the police to try and find him, at least for a little bit. It only took them a few months to call the case cold.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Claimed they turned the city inside out and came up empty. 'Presumed dead'.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "I didn't believe it for a second. From the moment I got that funeral invitation, I knew he was alive out there somewhere. I just had to find him.",
                name = "Mari"
            },
            new Dialouge
            {
                text = "But now? It's been three years, and every avenue I have to search the city's been exhausted. I'm starting to think he might be dead after all. If only-",
                name = "Mari"
            },
            new Dialouge
            {
                text = "Hellooooooo? Mari? You in there at all?",
                name = "Nora",
                portraitImg = "Nora",
            },
            new Dialouge
            {
                text = "Hey! Earth to Mari!",
                name = "Nora",
                portraitImg = "Nora",
            },
            new Dialouge
            {
                text = "h- whuh?",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "You're falling asleep again. Come downstairs, there's something important we need to talk about.",
                name = "Nora",
                portraitImg = "Nora",
            }
        };
        GUIManager.Instance.textbox.CreateTextSequence(dialouges);
        yield return new WaitForEndOfFrame();
        GUIManager.Instance.transitionScreen.GetComponent<Animator>().enabled = false;
        GUIManager.Instance.transitionScreen.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        while (GUIManager.Instance.textbox.textArrays == dialouges)
        {
            yield return new WaitForEndOfFrame();
        }

        GUIManager.Instance.transitionScreen.GetComponent<Animator>().enabled = true;
        GUIManager.Instance.Transition(true);
        GameManager.Instance.Player.GetComponent<Player>().allowControl = true;
    }
    IEnumerator confrontationDialouge()
    {
        //GameManager.Instance.Player.GetComponent<Player>().allowControl = false;
        GameManager.Instance.Player.SetActive(false);
        Animator animator = GUIManager.Instance.Transition(false);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("TransOut") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }

        Dialouge[] dialouges = new Dialouge[]
        {
            new Dialouge
            {
                text = "I gotta get out of here!",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "Indeed you do.",
            },
            new Dialouge
            {
                text = "Who's there? Show yourself!",
                name = "Mari",
                portraitImg = "Mari",
            },
			// A light in the center of the room should turn on, and Mako's portrait (but not name!) should become visible.
			new Dialouge
            {
                text = "Welcome, Mari. You've made the mistake of wandering into my provisional office. Now, you're going to explain why you're here and how the hell you got in.",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "How do you know my name!?",
                name = "Mari",
                portraitImg = "Mari",
            },
			// Mako's name should become visible.
			new Dialouge
            {
                text = "Isn't it obvious? It's me, Mako.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "Wait, Mako!? Where have you been?! I've been looking for you for the past three years! Everyone thinks you're dead!",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "That's precisely the point. No one - especially you - was supposed to be able to find me.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "But why?",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "This business is... lucrative, shall we say. And dangerous.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "I didn't want to drag anyone I knew into it with me, or get them caught up in a mess as a result of my work. So I ditched.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "I didn't really have a choice in the matter, anyway.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "'Work'?",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "Do I really have to spell it out now? I'm the boss of the yakuza you've waltzed into the headquarters for.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "Wha-",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "Shut up. I'm getting sick of this conversation, so I'm gonna cut to the chase: I can't just allow you to leave knowing what I've told you.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "That said, I'm not a heartless person, and I'd really rather not kill you.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "So here's the deal: I can pull some strings, make you a member of my yakuza, and you can leave on the condition you keep your mouth shut;",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "Or you die. Here and now.",
                name = "Mako",
                portraitImg = "mako1",
            },
            new Dialouge
            {
                text = "Are you out of your mind!?",
                name = "Mari",
                portraitImg = "Mari",
            },
            new Dialouge
            {
                text = "No, I'm giving you a way out. What'll it be?",
                name = "Mako",
                portraitImg = "mako1",
            }
        };
        GUIManager.Instance.textbox.CreateTextSequence(dialouges);
        yield return new WaitForEndOfFrame();
        while (GUIManager.Instance.textbox.textArrays == dialouges)
        {
            yield return new WaitForEndOfFrame();
        }

        int picked = int.MinValue;
        GUIManager.Instance.textbox.Choice(new string[] { "[1] Join", "[2] Refuse" }, (s) =>
        {
            picked = s;
            print(picked);
        });
        while (picked == int.MinValue)
        {
            yield return new WaitForEndOfFrame();
        }
        if (picked == 1)
        {
            dialouges = new Dialouge[] {
                new Dialouge
                {
                    text = "You don't give me much of a choice, do you?",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Fine. I'll join your yakuza.",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Excellent decision. I expect you to return here at the same time tomorrow so I can fill you in on how things work around here.",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                new Dialouge
                {
                    text = "If you don't... You know what's going to happen.",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                new Dialouge
                {
                    text = "Mari left the building, unprovoked by the guards, as if they knew of what occurred in the office...",
                },
				// Screen fades to black.
				new Dialouge
                {
                    text = "Well, you got back to the car in one piece. Find anything useful to our search?",
                    name = "Nora",
                    portraitImg = "Nora",
                },
                new Dialouge
                {
                    text = "... Not really. The place was swarming with yakuza thugs, so I didn't have much time to look around.",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "There's yakuza in there? Shit, we better get moving then!",
                    name = "Nora",
                    portraitImg = "Nora",
                },
                new Dialouge
                {
                    text = "In more ways than one, sure.",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Huh?",
                    name = "Nora",
                    portraitImg = "Nora",
                },
                new Dialouge
                {
                    text = "Don't worry about it. Just get us away from here before we get shot at or something!",
                    name = "Mari",
                    portraitImg = "Mari",
                }
            };
        }
        else
        {
            dialouges = new Dialouge[] {
                new Dialouge
                {
                    text = "...No.",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "This isn't a joke, Mari. I'm going to ask you aga-",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                new Dialouge
                {
                    text = "No. I can't do that, this isn't like you!",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Then you leave me with no other choi-",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                // Mari moves to the door and leaves the scene.
                new Dialouge
                {
                    text = "*Mari sprints away from the door.*",
                },
                new Dialouge
                {
                    text = "HEY! WHERE DO YOU THINK YOU'RE GOING?",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                new Dialouge
                {
                    text = "SOMEONE STOP THAT GIRL!",
                    name = "Mako",
                    portraitImg = "mako1",
                },
                // Screen fades to black.
                new Dialouge
                {
                    text = "*As Mari runs she's shot at by Mako's goons*",
                },
                new Dialouge
                {
                    text = "*She quickly jumps back into the car, unharmed but visibly exhausted.*",
                },
                new Dialouge
                {
                    text = "Mari, are you okay? What's going on?!",
                    name = "Nora",
                    portraitImg = "Nora",
                },
                new Dialouge
                {
                    text = "Go. We need to leave, NOW.",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Seriously, what-",
                    name = "Nora",
                    portraitImg = "Nora",
                },
                new Dialouge
                {
                    text = "I'll tell you when it's safe! We need to get out of here before we start getting shot at!",
                    name = "Mari",
                    portraitImg = "Mari",
                },
                new Dialouge
                {
                    text = "Alright, you don't need to say anything else! I'm on it!",
                    name = "Nora",
                    portraitImg = "Nora",
                }
            };
        }
        GUIManager.Instance.textbox.CreateTextSequence(dialouges);
        yield return new WaitForEndOfFrame();
        while (GUIManager.Instance.textbox.textArrays == dialouges)
        {
            yield return new WaitForEndOfFrame();
        }
        GameManager.Instance.TransitionToScene("credits");
    }
}
