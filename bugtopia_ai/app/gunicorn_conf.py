import json  # JSON 모듈을 가져와서 로그 데이터를 JSON 형식으로 출력할 때 사용
import multiprocessing  # 멀티프로세싱을 위한 모듈, CPU 코어 수 등을 처리
import os  # 환경변수 읽기를 위한 모듈

# 환경변수에서 WORKERS_PER_CORE 값을 가져오거나 기본값 '1'을 사용
workers_per_core_str = os.getenv("WORKERS_PER_CORE", "1")

# 환경변수에서 WEB_CONCURRENCY 값을 가져옴. 웹 서버에서 사용할 동시 작업자 수
web_concurrency_str = os.getenv("WEB_CONCURRENCY", None)

# 환경변수에서 HOST 값을 가져오거나 기본값 '0.0.0.0'을 사용
host = os.getenv("HOST", "0.0.0.0")

# 환경변수에서 PORT 값을 가져오거나 기본값 '80'을 사용
port = os.getenv("PORT", "80")

# 환경변수에서 BIND 값을 가져오거나 기본값 None
bind_env = os.getenv("BIND", None)

# 환경변수에서 LOG_LEVEL 값을 가져오거나 기본값 'info'를 사용
use_loglevel = os.getenv("LOG_LEVEL", "info")

# bind_env가 존재하면 그것을 사용하고, 아니면 HOST와 PORT를 조합하여 bind 설정
use_bind = bind_env or f"{host}:{port}"

# 현재 시스템의 CPU 코어 수를 얻음
cores = multiprocessing.cpu_count()

# WORKERS_PER_CORE 값을 실수로 변환 (예: "2" -> 2.0)
workers_per_core = float(workers_per_core_str)

# 기본 웹 동시 작업자 수는 각 코어당 작업자 수 * CPU 코어 수
default_web_concurrency = workers_per_core * cores

# WEB_CONCURRENCY 환경변수가 존재하면 해당 값으로 설정하고, 값이 0 이하이면 오류 발생
if web_concurrency_str:
    web_concurrency = int(web_concurrency_str)
    if web_concurrency <= 0:
        raise ValueError(f"Invalid WEB_CONCURRENCY value: {web_concurrency}. It must be greater than 0.")
else:
    # WEB_CONCURRENCY가 없다면 기본값인 default_web_concurrency 값으로 설정, 최소값은 2
    web_concurrency = max(int(default_web_concurrency), 2)

# 최종 로그 설정 값
loglevel = use_loglevel
workers = web_concurrency  # 실제 동작할 웹 서버의 작업자 수
bind = use_bind  # 바인딩할 호스트와 포트
keepalive = 120  # 연결을 유지할 시간 (초 단위)
errorlog = "-"  # 에러 로그는 표준 출력을 사용

# 로그 출력용 데이터를 딕셔너리 형태로 구성
log_data = {
    "loglevel": loglevel,  # 로그 레벨
    "workers": workers,  # 작업자 수
    "bind": '%s:%s' % (host, port),  # 바인딩된 호스트와 포트
    "worker_class": "uvicorn.workers.UvicornWorker",  # 사용될 워커 클래스 (Uvicorn 워커)
    "workers_per_core": workers_per_core,  # 코어당 작업자 수
    "host": host,  # 호스트
    "port": port,  # 포트
}

# 로그 데이터를 JSON 형식으로 출력
print(json.dumps(log_data))
