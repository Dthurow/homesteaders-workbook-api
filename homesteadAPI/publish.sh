set -x
docker build ./.. --pull -f ./Dockerfile -t registry.heroku.com/obscure-lowlands-53687/web
docker push registry.heroku.com/obscure-lowlands-53687/web
heroku container:release web