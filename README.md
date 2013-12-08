These C# .net classes are designed to extract unoccupied space from groups of closed pairs within a data feed.

Given a group of closed pairs, where each pair of the group comprises the following values:

fromInt  int64 

toInt int64

GivenID int  --simply a row identifier

GroupID -- a means of identify which group within a single stream the given item belongs to.

as long as the fromInt is always a smaller value than the toInt the pairs are considered closed.  

The GivenID is actually cursory and is intended to represent a unique identifier.

The GroupID identifies which "universe" the pair actually belongs to.  Its purpose is to keep the solution 
scaleable, so that pairs that belong to different groups can be part of the same feed.

The pairs within a group can demonstrate the following patterns

|------------|  |----------|  two pairs with a gap  e.g. (1,5) (7,13)

|---------|  overlap  (1,4)   

        |-------|  (3,7)

and most importantly enclosure

|------------------------------|   (1,16)

   |--------|  |--------| (2,5)  (6,10)


In these examples they are graphically represented in order, but do not need to be for purposes of data exchange.
The "span" of a group is determined by the left most value  (lowest fromInt) of any pair in the group, in conjunction 
with the right most (highest toInt).

Enclosure leaves us with the vital property that the pairs do not start and end in the same order.  Consider the 
enclosure example above.  The (1,16) pair is the first one to start, but it is the third one to end.
This indicates that the distinctiveness of the pair is not attributed to the space it occupies within the span, but rather
to its specifc boundaries.  The pairs can occupy the same space, but are still distinct and identifiable.
Because of this, identifying empty space within a group can NOT be accomplished by evaluating the relationship 
between any given pair and the one that started before it.  In the above example the space between the (2,5)
and the (6,10) is occupied by the (1,16) and does not count as a gap.

These classes present two algorithms to address this, both with O(Nlog(N)) runtime.
One creates a derived group of pairs that represent the inclusive space unoccupied by any pair in a group, 
the other the fusion, so that enclosed pairs are annulled.





