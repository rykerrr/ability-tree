Talent tree (essentially functions as a weighted digraph) featuring:
- Smooth filling for talent links when leveling talents
If the next talent requires 10 points in the previous one, the ui link between them is filled up by queueing tweening
- Link weight (talents require x amount of points in previous talent)
- Talents as ScriptableObject assets
- Eventual dynamic creation of the UI for the talents based on references in the talent assets
