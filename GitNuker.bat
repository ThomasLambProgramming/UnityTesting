git filter-branch --prune-empty -f --index-filter "git rm -rf --cached --ignore-unmatch Assets/Plugins/" --tag-name-filter cat -- --all
git for-each-ref --format="delete %(refname)" refs/original | git update-ref --stdin

git reflog expire --expire=now --all
git gc --prune=now
git push --force --all
git push --force --tags
