package geode.nativesample;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import org.apache.geode.cache.Declarable;
import org.apache.geode.cache.EntryEvent;
import org.apache.geode.cache.util.CacheListenerAdapter;

public class MyCacheListener extends CacheListenerAdapter<Integer, Object> implements Declarable {

	private ExecutorService executor = Executors.newFixedThreadPool(2);

	@Override
	public void afterCreate(EntryEvent<Integer, Object> event) {
		executor.execute(() -> {
			Integer key = event.getKey();
			Object value = event.getNewValue();
			Response res = new Response(key, value.toString());
			event.getRegion().getRegionService().getRegion("reply").put(key, res);
			System.out.println(String.format("afterCreate:%s", res.toString()));
		});
	}

	@Override
	public void afterUpdate(EntryEvent<Integer, Object> event) {
		executor.execute(() -> {
			Integer key = event.getKey();
			Object value = event.getNewValue();
			Response res = new Response(key, value.toString());
			event.getRegion().getRegionService().getRegion("reply").put(key, res);
			System.out.println(String.format("afterUpdate:%d=%s", key, res.toString()));
		});
	}
}
