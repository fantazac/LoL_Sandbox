public enum AbilityEffectType
{
    //Nothing for abilities that don't have effects (ex. Lucian E), will need to see if it's a problem or not

    AREA_OF_EFFECT,
    BASIC_ATTACK,
    DAMAGE_OVER_TIME,//ignite, 
    HEALING,//soraka w, sona w
    MULTI_HIT,//fiddlesticks e, brand r
    ATTACK_EFFECT,//on-hit
    REDIRECTED_DAMAGE,//thornmail, rammus w
    SHIELD,//lulu e, janna e
    SINGLE_TARGET,
    SPLASH_DAMAGE,//AoE, probably not used
}
