# covr-med

GitHub repository with the Unity 5.1.1 code for the covr-med project with the IBISC laboratory.


## To do list

(Add there the details of the tasks to do in the current branch)

* Create the working branches develop and hotfix


## Resources used

Here are the dependencies, relevent links and the external resources used for this project.


### Dependencies

* [Unity 5.1.0](https://unity3d.com/fr/get-unity/download/archive)


### Relevent links

* [Tips for Unity](https://www.gamasutra.com/blogs/HermanTulleken/20160812/279100/50_Tips_and_Best_Practices_for_Unity_2016_Edition.php)
* [Merge or Rebase?](https://www.atlassian.com/git/tutorials/merging-vs-rebasing)


### External resources

* None (for now...)


## About Git

### Branching

To add a new branch in the local repository
```bash
git checkout -b name_of_the_branch_00x
```

To push a branch to the distant repository
```bash
git push -u origin name_of_the_branch_00x
```

To delete a branch in the local repository
```bash
git branch -d name_of_the_branch_00x
```

To delete a branch in the distant repository
```bash
git push -d origin name_of_the_branch_00x
```


### Commiting

Adding all the changed / deleted / created files to the next commit
```bash
git add -A
```

Adding only the changed and deleted files to the next commit
```bash
git add -u
```

Commiting the changes
```bash
git commit -m "Commit message"
```

Changing the latest commit **IF IT IS NOT PUSHED ALREADY**
```bash
git commit --amend "Updated commit message"
```


### Merging

To merge a branch with a second one (fast-forward)
```bash
git checkout name_of_the_branch_to_merge_into
git merge name_of_the_branch_to_merge_from
```


### Stashing

To keep safe a series of changes before a checkout
```bash
git stash save
```

To get back the stashed modifications
```bash
git stash pop number_of_the_stash_to_apply_and_drop
```
