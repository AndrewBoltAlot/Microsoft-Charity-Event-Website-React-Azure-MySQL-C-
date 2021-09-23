# Git Usage

**Shaoshu Zhu**



This documentation is for introducing git as the version control tool for our project and as a manual to find solutions when getting stunned at somewhere mysterious.



Before start using git, the first thing you might need to do is:

```text
$ git config --global user.name "your preferred name"  
$ git config --global user.email "your work email" 
```



As we are using GitHub, I would recommend using **GitHub Desktop** or any other visualisation tool for github to trace commits in our repository.



## References

Basic tutorial:

[Git tutorial](https://www.atlassian.com/git/tutorials/what-is-version-control)

Book reference:

[Pro Git](https://git-scm.com/book/en/v2)

Angular format for commit:

[Angular official doc](https://gist.github.com/brianclements/841ea7bffdb01346392c)

[Understanding Semantic Commit Messages Using Git and Angular](https://nitayneeman.com/posts/understanding-semantic-commit-messages-using-git-and-angular/)



## Common useful commands

<img src="https://git-scm.com/book/en/v2/images/areas.png" alt="工作区、暂存区以及 Git 目录。" style="zoom:50%;" />

* ` git init` : Init a git repository. What does it actually do is making a `.git` folder in the folder in which you use this command to trace versions of contents.

* `git status`: Show the situation you are in.(On which branch, untracked files, files to be committed etc.)

* `git branch <-a> <-d> <branch name>`: Create a new branch with name.

  if `<-a>`: Show local and remote branches in this repo.

  if `<-d> <branch name>`: Delete local branch.

* `git checkout <branch name>`: Switch to a specific branch.

  

* `git add <.> <-A> <file>` : Add files to **Staging Area** but not commit them.

* `git commit -m "Your commit message with Angular format"`: 

  Commit changes to **Repository**, a commit message is important as this is the way other teammates understand your changes. 

  Tips: If you want to write multiple lines you can use this command without `-m` syntax so that Git would let you get in a page in which you write them.

* `git merge <branch name>`: Merge specific branch into current branch.

* `git reset <commit_hash> <-- filename>`: Switch to a specific commit.

* `git log <-p>`: Show commit logs.



## Work with remote Git server like GitHub

* `git remote <command> <remote_name> <remote_URL> `: For details see [git-remote doc](https://git-scm.com/docs/git-remote)

* `git clone <remote URL>`: Clone repo from servers.(I would suggest to use HTTPS URL)

* `git pull <branch_name> <remote_URL/remote_name>`: Get the latest version of a repo.(probably a specific branch)

* `git push <-all> <remote_URL/remote_name> <branch>`: Push local repo to remote repo.(Maybe a specific branch even it is not existed)

  

## Some scenarios

1. **Conflicts between local repo and remote repo**

   Make sure you use `git pull` **before** you use `git add`. Sometimes there are also problems happened when you doing so, always merge problems. You can choose to switch to a new branch and merge them locally or online.(or just stash your changes by using `git stash`)

2. **Submodule problem**

   When there are some `.git` folders in different working directory you might need to add some `.git` folders as submodule or just delete them.(Maybe lose some informations)



## Rules

* **Make sure** `master` and `dev` branches clean. (For example, no local configuration files, like `.DS_Store` for MacOS, `.VS-like` files for local IDE and other files irrelevant to deployment)

* **Push with creating a new branch for specific issue when this push is about code contribution.**

  For `dev` branch it would be always deployable and trying to not get messed up. When your code is related to an `#issue` it is better to firstly push your changes to branch `#issuenum-description`

  For example:

  There is an issue `#31 Add a new login microservice  `.

  Steps:

  1. `git branch 31-login_service`

  2. `git checkout 31-login_service`

  3. Make some contribution

  4. `git add -A`; `git commit -m "Angular format message"`; `git push origin 31-login_service`

  5. Let other reviewers know and review.(`git diff` or see the GUI on GitHub)

  6. After review, merge it to `dev` branch.

* **Create Docs with Markdown**

  It is important to trace the changes in Docs, if use Word or PDF it would be hard to do so.

  Markdown is a good and lightweight format for writing.

  