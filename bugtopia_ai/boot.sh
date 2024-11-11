MODULE=app.main
APP=app      
NAME=bugtopia_ai



#WORKER_THREADS: between 2-4 per core: https://docs.gunicorn.org/en/latest/settings.html?highlight=workers#workers

uvicorn ${MODULE}:${APP}        \
         --port 5000       \
         --host 0.0.0.0 \
         --workers $WORKER_THREADS
